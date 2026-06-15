using BookShop.Cart.Application;
using BookShop.Cart.Infrastructure;
using BookShop.Cart.Infrastructure.Inbox;
using BookShop.Users.IntegrationEvents;
using BuildingBlocks.Presentation.Endpoints;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Cart.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddCartModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpoints(typeof(DependencyInjection).Assembly);

        builder.Services.AddApplication();

        builder.AddInfrastructure();

        return builder.Services;
    }

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator, string instanceId)
    {
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<UserRegisteredIntegrationEvent>>()
            .Endpoint(c => c.InstanceId = instanceId);
    }
}
