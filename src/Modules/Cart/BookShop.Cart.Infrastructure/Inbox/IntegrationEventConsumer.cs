using System.Data.Common;
using System.Text.Json;
using BookShop.Shared;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Application.EventBus;
using BuildingBlocks.Infrastructure.Inbox;
using Dapper;
using MassTransit;

namespace BookShop.Cart.Infrastructure.Inbox;

public sealed class IntegrationEventConsumer<TIntegrationEvent>(
    IDbConnectionFactory dbConnectionFactory
) : IConsumer<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    public async Task Consume(ConsumeContext<TIntegrationEvent> context)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        TIntegrationEvent integrationEvent = context.Message;

        var inboxMessage = new InboxMessage
        {
            Id = integrationEvent.Id,
            Type = integrationEvent.GetType().FullName!,
            Content = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType()),
            OccurredOnUtc = integrationEvent.OccurredOnUtc
        };

        const string sql =
            $"""
             INSERT INTO {Services.Cart}.inbox_messages(id, type, content, occurred_on_utc)
             VALUES (@Id, @Type, @Content::json, @OccurredOnUtc)
             ON CONFLICT (id) DO NOTHING;
             """;

        await connection.ExecuteAsync(sql, inboxMessage);
    }
}
