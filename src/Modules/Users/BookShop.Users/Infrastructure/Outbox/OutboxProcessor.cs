using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Text.Json;
using BookShop.Shared;
using BookShop.Users.Domain;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Outbox;
using Dapper;
using Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookShop.Users.Infrastructure.Outbox;

public sealed class OutboxProcessor(
    IDbConnectionFactory dbConnectionFactory,
    IPublisher publisher,
    IOptions<OutboxJobOptions> outboxOptions,
    TimeProvider timeProvider,
    ILogger<OutboxProcessor> logger
)
{
    private static string SchemaName => "users";
    private static string ServiceName => "users";

    public async Task<int> ProcessAsync(CancellationToken cancellationToken = default)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("{Service} - Beginning to process outbox messages", ServiceName);
        }

        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        await using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        if (outboxMessages.Count == 0)
        {
            return 0;
        }

        var updateQueue = new ConcurrentQueue<OutboxUpdate>();
        var typeCache = new ConcurrentDictionary<string, Type>();

        await PublishMessagesAsync(outboxMessages, typeCache, updateQueue, cancellationToken);

        await UpdateOutboxMessagesAsync(connection, transaction, updateQueue);

        await transaction.CommitAsync(cancellationToken);

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("{Service} - Completed processing outbox messages", SchemaName);
        }

        return outboxMessages.Count;
    }

    private async Task PublishMessagesAsync(
        IReadOnlyList<OutboxMessageResponse> outboxMessages,
        ConcurrentDictionary<string, Type> typeCache,
        ConcurrentQueue<OutboxUpdate> updateQueue,
        CancellationToken cancellationToken
    )
    {
        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                Type messageType = GetOrAddMessageType(typeCache, outboxMessage.Type);
                object domainEvent = JsonSerializer.Deserialize(outboxMessage.Content, messageType)!;
                await publisher.Publish(domainEvent, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while processing outbox message {MessageId}", outboxMessage.Id);
                exception = ex;
            }

            updateQueue.Enqueue(new OutboxUpdate(outboxMessage.Id, timeProvider.GetUtcNow().UtcDateTime, exception?.ToString()));
        }
    }

    private async Task UpdateOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        ConcurrentQueue<OutboxUpdate> updateQueue)
    {
        if (updateQueue.IsEmpty)
        {
            return;
        }

        string updateSql =
            $$"""
                 UPDATE {{SchemaName}}.{{OutboxConstants.TableName}}
                 SET processed_on_utc = v.processed_on_utc,
                     error = v.error
                 FROM UNNEST(@Ids, @ProcessedAts, @Errors)
                    AS v(id, processed_on_utc, error)
                 WHERE {{SchemaName}}.{{OutboxConstants.TableName}}.id = v.id::uuid
              """;

        var parameters = new
        {
            Ids = updateQueue.Select(x => x.Id).ToArray(),
            ProcessedAts = updateQueue.Select(x => x.ProcessedOnUtc).ToArray(),
            Errors = updateQueue.Select(x => x.Exception).ToArray()
        };

        await connection.ExecuteAsync(updateSql, parameters, transaction);
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction
    )
    {
        string sql =
            $"""
             SELECT
                id      AS {nameof(OutboxMessageResponse.Id)},
                type    AS {nameof(OutboxMessageResponse.Type)},
                content AS {nameof(OutboxMessageResponse.Content)}
             FROM {SchemaName}.{OutboxConstants.TableName}
             WHERE processed_on_utc IS NULL
             ORDER BY occurred_on_utc
             LIMIT {outboxOptions.Value.BatchSize}
             FOR UPDATE SKIP LOCKED
             """;

        IEnumerable<OutboxMessageResponse> outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }


    private static Type GetOrAddMessageType(ConcurrentDictionary<string, Type> typeCache, string typeName)
    {
        return typeCache.GetOrAdd(typeName, name => AssemblyMarker.Assembly.GetType(name)!);
    }
}
