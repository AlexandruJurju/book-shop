namespace BuildingBlocks.Infrastructure.Inbox;

public record struct InboxUpdate(
    Guid Id,
    DateTime ProcessedOnUtc,
    string? Exception
);
