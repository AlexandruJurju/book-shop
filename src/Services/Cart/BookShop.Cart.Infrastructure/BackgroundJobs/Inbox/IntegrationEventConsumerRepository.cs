using BookShop.Cart.Application.Abstractions.EventBus;
using BookShop.Shared;
using BuildingBlocks.Infrastructure.Inbox;
using Dapper;
using Npgsql;

namespace BookShop.Cart.Infrastructure.BackgroundJobs.Inbox;

internal sealed class IntegrationEventConsumerRepository(
    NpgsqlDataSource dataSource
) : IIntegrationEventConsumerRepository
{
    public async Task<bool> ExistsAsync(Guid id, string consumerName)
    {
        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT EXISTS (
                 SELECT 1
                 FROM {Services.Cart}.inbox_message_consumers
                 WHERE inbox_message_id = @InboxMessageId
                  AND name = @Name
             );
             """;

        var parameters = new InboxMessageConsumer(id, consumerName);

        return await connection.ExecuteScalarAsync<bool>(sql, parameters);
    }

    public async Task AddAsync(Guid id, string consumerName)
    {
        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync();

        const string sql =
            $"""
             INSERT INTO {Services.Cart}.inbox_message_consumers(inbox_message_id, name)
             VALUES(@InboxMessageId, @Name);
             """;

        var parameters = new InboxMessageConsumer(id, consumerName);

        await connection.ExecuteAsync(sql, parameters);
    }
}
