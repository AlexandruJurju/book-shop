using BookShop.Users.Contracts;
using BuildingBlocks.Application.EventBus;

namespace BookShop.Cart.Application.IntegrationEventsHandlers;

public sealed class UserRegisteredIntegrationEventHandler : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public Task HandleAsync(UserRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine(42);
        return Task.CompletedTask;
    }
}
