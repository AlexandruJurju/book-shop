using BookShop.Users.Domain.Users.Events;
using BuildingBlocks.Application.CQRS;

namespace BookShop.Users.Application.Users.RegisterUser;

public sealed class UserRegisteredDomainEventHandler : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public async ValueTask Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"{nameof(UserRegisteredDomainEventHandler)} handle {notification.GetType().Name}");
    }
}
