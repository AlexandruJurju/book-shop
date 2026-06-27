using BookShop.Users.IntegrationEvents;
using BuildingBlocks.Application.EventBus;

namespace BookShop.Cart.Application.IntegrationEvents;

public sealed class UserRegisteredIntegrationEventHandler : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public async Task HandleAsync(UserRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine(67);
    }
}
