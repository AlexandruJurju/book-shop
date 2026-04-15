using BuildingBlocks.Application.EventBus;
using BuildingBlocks.Infrastructure.Configuration;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Constants.Aspire;

namespace BuildingBlocks.Infrastructure.EventBus;

public static class Extensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventBus, EventBus>();

        string rabbitMq = configuration.GetConnectionStringOrThrow(Resources.RabbitMQ);
        
        services.AddMassTransit(configure =>
        {
            configure.SetKebabCaseEndpointNameFormatter();

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMq);
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
