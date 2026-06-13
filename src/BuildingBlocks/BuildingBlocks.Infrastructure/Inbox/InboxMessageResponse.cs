namespace BuildingBlocks.Infrastructure.Inbox;

public sealed record InboxMessageResponse(
    Guid Id,
    string Type,
    string Content
);
