using System.Data;
using System.Data.Common;
using System.Text.Json;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Dapper;
using Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TickerQ.Utilities.Base;
using TickerQ.Utilities.Interfaces;

namespace BuildingBlocks.Infrastructure.Outbox;

public abstract partial class OutboxJobBase(
    IDbConnectionFactory dbConnectionFactory,
    IPublisher publisher,
    IOptionsMonitor<OutboxJobOptions> outboxOptions,
    TimeProvider timeProvider,
    ILogger logger
) : ITickerFunction
{
    protected abstract string ServiceName { get; }
    protected abstract string SchemaName { get; }

    private OutboxJobOptions Options => outboxOptions.Get(ServiceName);

    public async Task ExecuteAsync(TickerFunctionContext context, CancellationToken cancellationToken = default)
    {
        LogServiceBeginningToProcessOutboxMessages(ServiceName);

        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        await using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                IDomainEvent domainEvent = JsonSerializer.Deserialize<IDomainEvent>(outboxMessage.Content)!;

                await publisher.Publish(domainEvent, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while processing outbox message {MessageId}", outboxMessage.Id);
                exception = ex;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        await transaction.CommitAsync(cancellationToken);
        LogServiceCompletedProcessingOutboxMessages(ServiceName);
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        string sql =
            $"""
             SELECT
                id      AS {nameof(OutboxMessageResponse.Id)},
                content AS {nameof(OutboxMessageResponse.Content)}
             FROM {SchemaName}.{OutboxConstants.TableName}
             WHERE processed_on_utc IS NULL
             ORDER BY occurred_on_utc
             LIMIT {Options.BatchSize}
             FOR UPDATE SKIP LOCKED
             """;

        IEnumerable<OutboxMessageResponse> outboxMessages =
            await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        string sql =
            $"""
             UPDATE FROM {OutboxConstants.TableName}
             SET processed_on_utc = @ProcessedOnUtc,
                 error            = @Error
             WHERE id = @Id
             """;

        await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = timeProvider.GetUtcNow(),
                Error = exception?.ToString()
            },
            transaction);
    }

    [LoggerMessage(LogLevel.Information, "{Service} - Beginning to process outbox messages")]
    private partial void LogServiceBeginningToProcessOutboxMessages(string service);

    [LoggerMessage(LogLevel.Information, "{Service} - Completed processing outbox messages")]
    private partial void LogServiceCompletedProcessingOutboxMessages(string service);
}
