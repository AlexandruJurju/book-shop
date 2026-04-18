using System.Data;
using System.Data.Common;
using System.Text.Json;
using BookShop.Shared.Aspire;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using BuildingBlocks.Infrastructure.Outbox;
using Dapper;
using Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TickerQ.Utilities.Base;
using TickerQ.Utilities.Interfaces;

namespace BookShop.Users.Infrastructure.Outbox;

internal sealed partial class OutboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IPublisher publisher,
    IOptionsMonitor<OutboxJobOptions> outboxOptions,
    TimeProvider timeProvider,
    ILogger<OutboxJob> logger
) : ITickerFunction
{
    private OutboxJobOptions Options => outboxOptions.Get(Services.Users);

    public async Task ExecuteAsync(TickerFunctionContext context, CancellationToken cancellationToken = new())
    {
        string serviceName = Services.Users;
        LogServiceBeginningToProcessOutboxMessages(serviceName);

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

        LogServiceCompletedProcessingOutboxMessages(serviceName);
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
             FROM {Services.Users}.{OutboxConstants.TableName}
             WHERE processed_on_utc IS NULL
             ORDER BY occurred_on_utc
             LIMIT {Options.BatchSize}
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
    partial void LogServiceBeginningToProcessOutboxMessages(string service);

    [LoggerMessage(LogLevel.Information, "{Service} - Completed processing outbox messages")]
    partial void LogServiceCompletedProcessingOutboxMessages(string service);
}
