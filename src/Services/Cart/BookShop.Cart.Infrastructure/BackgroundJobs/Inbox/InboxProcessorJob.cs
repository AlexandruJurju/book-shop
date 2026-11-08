using System.Collections.Concurrent;
using System.Data;
using System.Text.Json;
using BookShop.Shared;
using BuildingBlocks.Application.EventBus;
using BuildingBlocks.Infrastructure.Inbox;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using Quartz;

namespace BookShop.Cart.Infrastructure.BackgroundJobs.Inbox;

[DisallowConcurrentExecution]
internal sealed class InboxProcessorJob(
    NpgsqlDataSource dataSource,
    IIntegrationEventsDispatcher integrationEventsDispatcher,
    IOptions<InboxJobOptions> inboxOptions,
    TimeProvider timeProvider,
    ILogger<InboxProcessorJob> logger
) : IJob
{
    private static string SchemaName => Services.Cart;

    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using NpgsqlTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        IReadOnlyList<InboxMessageResponse> inboxMessages = await GetInboxMessagesAsync(connection, transaction);

        ConcurrentQueue<InboxUpdate> updateQueue = await PublishMessagesAsync(inboxMessages, cancellationToken);

        if (!updateQueue.IsEmpty)
        {
            await UpdateInboxMessagesAsync(connection, transaction, updateQueue);
        }

        await transaction.CommitAsync(cancellationToken);
    }

    private async Task<IReadOnlyList<InboxMessageResponse>> GetInboxMessagesAsync(IDbConnection connection, IDbTransaction transaction)
    {
        string sql = $"""
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

    private async Task<ConcurrentQueue<InboxUpdate>> PublishMessagesAsync(
        IReadOnlyList<InboxMessageResponse> inboxMessages,
        CancellationToken cancellationToken
    )
    {
        var updateQueue = new ConcurrentQueue<InboxUpdate>();

        DateTime occuredAtUtc = timeProvider.GetUtcNow().UtcDateTime;

        foreach (InboxMessageResponse inboxMessage in inboxMessages)
        {
            try
            {
                Type eventType = IntegrationEventTypeCache.GetOrAdd(inboxMessage.Type);

                var deserializedIntegrationEvent = (IIntegrationEvent)JsonSerializer.Deserialize(inboxMessage.Content, eventType)!;

                await integrationEventsDispatcher.DispatchAsync(deserializedIntegrationEvent, cancellationToken);

                updateQueue.Enqueue(new InboxUpdate(inboxMessage.Id, occuredAtUtc, null));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while processing inbox message {MessageId}", inboxMessage.Id);
                updateQueue.Enqueue(new InboxUpdate(inboxMessage.Id, occuredAtUtc, ex.ToString()));
            }
        }

        return updateQueue;
    }

    private async Task UpdateInboxMessagesAsync(IDbConnection connection, IDbTransaction transaction, ConcurrentQueue<InboxUpdate> updateQueue)
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
            Errors = updateQueue.Select(x => x.Exception).ToArray(),
        };

        await connection.ExecuteAsync(updateSql, parameters, transaction);
    }
}
