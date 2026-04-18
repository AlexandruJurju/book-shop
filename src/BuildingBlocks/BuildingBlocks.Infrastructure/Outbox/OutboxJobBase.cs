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
    IOptions<OutboxJobOptions> outboxOptions,
    TimeProvider timeProvider,
    ILogger logger
) : ITickerFunction
{
    protected abstract string ModuleName { get; }
    protected abstract string SchemaName { get; }

    public async Task ExecuteAsync(TickerFunctionContext context, CancellationToken cancellationToken = new())
    {
        LogModuleBeginningToProcessOutboxMessages(ModuleName);

        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        await using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                IDomainEvent domainEvent =
                    JsonSerializer.Deserialize<IDomainEvent>(outboxMessage.Content)!;

                await publisher.Publish(domainEvent, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id
                );
                exception = ex;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        await transaction.CommitAsync(cancellationToken);

        LogModuleCompletedProcessingOutboxMessages(ModuleName);
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
             LIMIT {outboxOptions.Value.BatchSize}
             FOR UPDATE
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
             UPDATE {SchemaName}.outbox_messages
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

    [LoggerMessage(LogLevel.Information, "{Module} - Beginning to process outbox messages")]
    private partial void LogModuleBeginningToProcessOutboxMessages(string module);

    [LoggerMessage(LogLevel.Information, "{Module} - Completed processing outbox messages")]
    private partial void LogModuleCompletedProcessingOutboxMessages(string module);
}
