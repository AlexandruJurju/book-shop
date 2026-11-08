using BuildingBlocks.Application.EventBus;

namespace BookShop.Users.Contracts.IntegrationEvents;

public sealed record UserRegisteredIntegrationEvent(
    DateTime OccurredAtUtc,
    Guid UserId,
    string Email
) : IntegrationEvent(Guid.CreateVersion7(), OccurredAtUtc);
