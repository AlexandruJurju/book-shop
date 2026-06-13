using BookShop.Users.IntegrationEvents;
using BuildingBlocks.Application.EventBus;

namespace BookShop.Cart.Application.IntegrationEvents;

public sealed class UserRegisteredIntegrationEventHandler : IntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public override async Task Handle(UserRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine(67);
    }
}
