using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Text.Json;
using BookShop.Cart.Application;
using BookShop.Shared;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Application.EventBus;
using BuildingBlocks.Infrastructure.Inbox;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookShop.Cart.Infrastructure.Inbox;

internal sealed class InboxProcessor(
    IDbConnectionFactory dbConnectionFactory,
    IntegrationEventsDispatcher integrationEventsDispatcher,
    IOptions<InboxJobOptions> inboxOptions,
    TimeProvider timeProvider,
    ILogger<InboxProcessor> logger
)
{
    private static string SchemaName => Services.Cart;
    private static string ServiceName => Services.Cart;

    public async Task<int> ProcessAsync(CancellationToken cancellationToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("{Service} - Beginning to process inbox messages", ServiceName);
        }

        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        await using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        IReadOnlyList<InboxMessageResponse> inboxMessages = await GetInboxMessagesAsync(connection, transaction);

        if (inboxMessages.Count == 0)
        {
            return 0;
        }

        var updateQueue = new ConcurrentQueue<InboxUpdate>();

        await PublishMessagesAsync(inboxMessages, updateQueue, cancellationToken);

        await UpdateInboxMessagesAsync(connection, transaction, updateQueue);

        await transaction.CommitAsync(cancellationToken);

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("{Service} - Completed processing inbox messages", SchemaName);
        }

        return inboxMessages.Count;
    }

    private async Task<IReadOnlyList<InboxMessageResponse>> GetInboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction
    )
    {
        string sql =
            $"""
             SELECT
                id      AS {nameof(InboxMessageResponse.Id)},
                type    AS {nameof(InboxMessageResponse.Type)},
                content AS {nameof(InboxMessageResponse.Content)}
             FROM {SchemaName}.{InboxConstants.TableName}
             WHERE processed_on_utc IS NULL
             ORDER BY occurred_on_utc
             LIMIT {inboxOptions.Value.BatchSize}
             FOR UPDATE SKIP LOCKED
             """;

        IEnumerable<InboxMessageResponse> inboxMessages = await connection.QueryAsync<InboxMessageResponse>(sql, transaction: transaction);

        return inboxMessages.ToList();
    }

    private async Task PublishMessagesAsync(
        IReadOnlyList<InboxMessageResponse> inboxMessages,
        ConcurrentQueue<InboxUpdate> updateQueue,
        CancellationToken cancellationToken)
    {
        foreach (InboxMessageResponse inboxMessage in inboxMessages)
        {
            Exception? exception = null;
            try
            {
                Type eventType = IntegrationEventTypeCache.GetOrAdd(inboxMessage.Type);

                var integrationEvent = (IIntegrationEvent)JsonSerializer.Deserialize(inboxMessage.Content, eventType)!;

                await integrationEventsDispatcher.DispatchAsync([integrationEvent], cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while processing inbox message {MessageId}", inboxMessage.Id);
                exception = ex;
            }

            updateQueue.Enqueue(new InboxUpdate(
                inboxMessage.Id,
                timeProvider.GetUtcNow().UtcDateTime,
                exception?.ToString()));
        }
    }

    private async Task UpdateInboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        ConcurrentQueue<InboxUpdate> updateQueue)
    {
        if (updateQueue.IsEmpty)
        {
            return;
        }

        string updateSql =
            $$"""
                 UPDATE {{SchemaName}}.{{InboxConstants.TableName}}
                 SET processed_on_utc = v.processed_on_utc,
                     error = v.error
                 FROM UNNEST(@Ids, @ProcessedAts, @Errors)
                    AS v(id, processed_on_utc, error)
                 WHERE {{SchemaName}}.{{InboxConstants.TableName}}.id = v.id::uuid
              """;

        var parameters = new
        {
            Ids = updateQueue.Select(x => x.Id).ToArray(),
            ProcessedAts = updateQueue.Select(x => x.ProcessedOnUtc).ToArray(),
            Errors = updateQueue.Select(x => x.Exception).ToArray()
        };

        await connection.ExecuteAsync(updateSql, parameters, transaction);
    }
}
