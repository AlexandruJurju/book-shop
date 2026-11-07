using BuildingBlocks.Domain;

namespace BookShop.Users.Domain.Users.Events;

public sealed class UserRegisteredDomainEvent(Guid userid) : DomainEvent
{
    public Guid UserId { get; init; } = userid;
}
