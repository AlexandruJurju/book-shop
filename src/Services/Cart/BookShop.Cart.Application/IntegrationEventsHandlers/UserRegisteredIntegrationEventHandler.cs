using BookShop.Users.Contracts.IntegrationEvents;
using BuildingBlocks.Application.EventBus;

namespace BookShop.Cart.Application.IntegrationEventsHandlers;

public sealed class UserRegisteredIntegrationEventHandler
{
    public async Task HandleAsync(UserRegisteredIntegrationEvent message, CancellationToken cancellationToken)
    {
        Console.WriteLine(message.Email);

        await Task.Delay(1000, cancellationToken);
    }
}
