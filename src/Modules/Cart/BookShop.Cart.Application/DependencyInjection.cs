using BookShop.Cart.Application.EventBus;
using BookShop.Cart.Application.IntegrationEvents;
using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Cart.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add all Integration Event handlers
        services.Scan(scan => scan
            .FromAssemblyOf<UserRegisteredIntegrationEventHandler>()
            .AddClasses(c => c.AssignableTo(typeof(IIntegrationEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Decorate all integration event handlers
        services.Decorate(
            typeof(IIntegrationEventHandler<>),
            typeof(IdempotentIntegrationEventHandler<>));

        return services;
    }
}
