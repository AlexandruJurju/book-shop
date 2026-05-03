using System.Reflection;
using BookShop.Shared;
using BuildingBlocks.Application.EventBus;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BuildingBlocks.Infrastructure.EventBus;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomMassTransit(
        this IServiceCollection services,
        Action<IRegistrationConfigurator, string>[] moduleConfigureConsumers
    )
    {
        services.TryAddSingleton<IEventBus, EventBus>();

        services.AddMassTransit(configure =>
        {
            string serviceName = "BookShop.Api";
            string instanceId = serviceName.ToLowerInvariant().Replace('.', '-');
            foreach (Action<IRegistrationConfigurator, string> configureConsumers in moduleConfigureConsumers)
            {
                configureConsumers(configure, instanceId);
            }

            configure.SetKebabCaseEndpointNameFormatter();

            configure.UsingRabbitMq((context, cfg) =>
            {
                string connectionString = context
                    .GetRequiredService<IConfiguration>()
                    .GetConnectionString(Resources.RabbitMq)!;

                cfg.Host(new Uri(connectionString!));
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    public static IServiceCollection AddCustomMassTransit(
        this IServiceCollection services
    )
    {
        services.TryAddSingleton<IEventBus, EventBus>();

        services.AddMassTransit(configure =>
        {
            configure.AddConsumers(Assembly.GetExecutingAssembly());
            configure.SetKebabCaseEndpointNameFormatter();

            configure.UsingRabbitMq((context, cfg) =>
            {
                string connectionString = context
                    .GetRequiredService<IConfiguration>()
                    .GetConnectionString(Resources.RabbitMq)!;

                cfg.Host(new Uri(connectionString!));
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
