using BookShop.Cart.Application.Abstractions;
using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Cart.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IIntegrationEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Decorate(typeof(IIntegrationEventHandler<>), typeof(IdempotentIntegrationEventHandler<>));

        return services;
    }
}
