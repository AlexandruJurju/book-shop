using System.Text.Json;
using BookShop.Shared;
using BuildingBlocks.Application.EventBus;
using BuildingBlocks.Infrastructure.Inbox;
using Dapper;
using MassTransit;
using Npgsql;

namespace BookShop.Cart.Infrastructure.BackgroundJobs.Inbox;

internal sealed class InboxConsumer<TIntegrationEvent>(
    NpgsqlDataSource dataSource
) : IConsumer<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    private const string SchemaName = Services.Cart;

    public async Task Consume(ConsumeContext<TIntegrationEvent> context)
    {
        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync();

        TIntegrationEvent integrationEvent = context.Message;

        var inboxMessage = new InboxMessage
        {
            Id = integrationEvent.Id,
            Type = integrationEvent.GetType().FullName!,
            Content = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType()),
            OccurredOnUtc = integrationEvent.OccurredAtUtc,
        };

        const string sql =
            $"""
             INSERT INTO {SchemaName}.inbox_messages(id, type, content, occurred_on_utc)
             VALUES(@Id, @Type, @Content::json, @OccurredOnUtc)
             ON CONFLICT (id) DO NOTHING;
             """;

        await connection.ExecuteAsync(sql, inboxMessage);
    }
}
