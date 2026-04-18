namespace BuildingBlocks.Infrastructure.Outbox;

public sealed record OutboxMessageResponse(Guid Id, string Content);
