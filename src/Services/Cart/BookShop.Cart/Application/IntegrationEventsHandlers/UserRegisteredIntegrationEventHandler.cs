using BookShop.Users.Contracts.IntegrationEvents;
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
