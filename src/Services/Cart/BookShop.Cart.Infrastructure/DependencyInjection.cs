using BookShop.Cart.Application;
using BookShop.Cart.Application.Abstractions.Data;
using BookShop.Cart.Application.EventBus;
using BookShop.Cart.Infrastructure.EntityFramework;
using BookShop.Cart.Infrastructure.Inbox;
using BookShop.Shared;
using BookShop.Users.IntegrationEvents;
using BuildingBlocks.Application.EventBus;
using BuildingBlocks.Infrastructure.EntityFramework;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TickerQ.DependencyInjection;

namespace BookShop.Cart.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;
        IConfigurationManager configuration = builder.Configuration;

        builder.AddCustomPostgresDbContext<CartDbContext>(Resources.Postgres, Services.Cart);
        services.AddScoped<ICartDbContext>(provider => provider.GetRequiredService<CartDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CartDbContext>());

        services.AddIntegrationEventHandlers();
        services.AddScoped<IntegrationEventsDispatcher>();
        services.AddScoped<IIntegrationEventConsumerRepository, IntegrationEventConsumerRepository>();
        AddInboxJob(services, configuration);
    }

    private static void AddInboxJob(IServiceCollection services, IConfigurationManager configuration)
    {
        IConfigurationSection section = configuration
            .GetRequiredSection($"Jobs:{InboxJobOptions.ConfigurationSection}");

        services
            .AddOptionsWithValidateOnStart<InboxJobOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<InboxProcessor>();
        services.AddScoped<InboxProcessorJob>();

        InboxJobOptions inboxJobOptions = section.Get<InboxJobOptions>()!;

        services
            .MapTicker<InboxProcessorJob>()
            .WithMaxConcurrency(1)
            .WithCron(inboxJobOptions.Cron);
    }

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator, string instanceId)
    {
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<UserRegisteredIntegrationEvent>>()
            .Endpoint(c => c.InstanceId = instanceId);
    }

    private static void AddIntegrationEventHandlers(this IServiceCollection services)
    {
        Type[] integrationEventHandlers = AssemblyMarker.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToArray();

        foreach (Type integrationEventHandler in integrationEventHandlers)
        {
            services.TryAddScoped(integrationEventHandler);

            Type integrationEvent = integrationEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler =
                typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);

            services.Decorate(integrationEventHandler, closedIdempotentHandler);
        }
    }
}
