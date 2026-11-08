using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Text.Json;
using BookShop.Shared;
using BuildingBlocks.Application.Abstractions.Data;
using BuildingBlocks.Domain;
using BuildingBlocks.Infrastructure.Outbox;
using Dapper;
using Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace BookShop.Users.Infrastructure.BackgroundJobs.Outbox;

[DisallowConcurrentExecution]
internal sealed class OutboxProcessorJob(
    IDbConnectionFactory dbConnectionFactory,
    IMediator mediator,
    IOptions<OutboxJobOptions> outboxOptions,
    TimeProvider timeProvider,
    ILogger<OutboxProcessorJob> logger
) : IJob
{
    private static string SchemaName => Services.Users;

    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        await using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        if (outboxMessages.Count == 0)
        {
            return;
        }

        var updateQueue = new ConcurrentQueue<OutboxUpdate>();

        await PublishMessagesAsync(outboxMessages, updateQueue, cancellationToken);

        await UpdateOutboxMessagesAsync(connection, transaction, updateQueue);

        await transaction.CommitAsync(cancellationToken);
    }

    private async Task PublishMessagesAsync(
        IReadOnlyList<OutboxMessageResponse> outboxMessages,
        ConcurrentQueue<OutboxUpdate> updateQueue,
        CancellationToken cancellationToken
    )
    {
        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                Type messageType = DomainEventTypeCache.GetOrAdd(outboxMessage.Type);
                var domainEvent = (IDomainEvent)JsonSerializer.Deserialize(outboxMessage.Content, messageType)!;
                await mediator.Publish(domainEvent, cancellationToken);
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
        ConcurrentQueue<OutboxUpdate> updateQueue
    )
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
}
