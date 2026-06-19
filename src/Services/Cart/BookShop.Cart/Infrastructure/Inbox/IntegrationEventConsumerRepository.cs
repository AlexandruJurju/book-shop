using System.Data.Common;
using BookShop.Cart.Application.EventBus;
using BookShop.Shared;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Inbox;
using Dapper;

namespace BookShop.Cart.Infrastructure.Inbox;

internal sealed class IntegrationEventConsumerRepository(
    IDbConnectionFactory dbConnectionFactory
) : IIntegrationEventConsumerRepository
{
    public async Task<bool> InboxConsumerExistsAsync(Guid id, string name)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT EXISTS(
                 SELECT 1
                 FROM {Services.Cart}.inbox_message_consumers
                 WHERE inbox_message_id = @InboxMessageId AND
                       name = @Name
             )
             """;

        var inboxMessageConsumer = new InboxMessageConsumer(id, name);

        return await dbConnection.ExecuteScalarAsync<bool>(sql, inboxMessageConsumer);
    }

    public async Task InsertConsumerAsync(Guid id, string name)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             INSERT INTO {Services.Cart}.inbox_message_consumers(inbox_message_id, name)
             VALUES (@InboxMessageId, @Name)
             """;

        var inboxMessageConsumer = new InboxMessageConsumer(id, name);

        await dbConnection.ExecuteAsync(sql, inboxMessageConsumer);
    }
}
