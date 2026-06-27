using System.Data.Common;
using BookShop.Shared;
using BookShop.Users.Application.Abstractions.Idempotency;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Outbox;
using Dapper;

namespace BookShop.Users.Infrastructure.Idempotency;

internal sealed class DomainEventConsumerRepository(
    IDbConnectionFactory dbConnectionFactory
) : IDomainEventConsumerRepository
{
    public async Task<bool> ExistsAsync(Guid id, string consumerName, CancellationToken cancellationToken)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT EXISTS(
                 SELECT 1
                 FROM {Services.Users}.outbox_message_consumers
                 WHERE outbox_message_id = @OutboxMessageId AND
                       name = @Name
             )
             """;

        var consumer = new OutboxMessageConsumer(id, consumerName);

        return await dbConnection.ExecuteScalarAsync<bool>(sql, consumer);
    }

    public async Task AddAsync(Guid id, string consumerName, CancellationToken cancellationToken)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             INSERT INTO {Services.Users}.outbox_message_consumers(outbox_message_id, name)
             VALUES (@OutboxMessageId, @Name)
             """;

        var inboxMessageConsumer = new OutboxMessageConsumer(id, consumerName);

        await dbConnection.ExecuteAsync(sql, inboxMessageConsumer);
    }
}
