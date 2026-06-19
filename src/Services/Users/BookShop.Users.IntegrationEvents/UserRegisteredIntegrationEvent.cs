using BuildingBlocks.Application.EventBus;

namespace BookShop.Users.IntegrationEvents;

public sealed class UserRegisteredIntegrationEvent(
    Guid id,
    DateTime occurredOnUtc,
    Guid userId,
    string email
) : IntegrationEvent(id, occurredOnUtc)
{
    public Guid UserId { get; init; } = userId;
    public string Email { get; init; } = email;
}
