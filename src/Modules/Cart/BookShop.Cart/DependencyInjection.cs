using BookShop.Cart.Application.Abstractions.Data;
using BookShop.Cart.Application.EventBus;
using BookShop.Cart.Application.IntegrationEvents;
using BookShop.Cart.Infrastructure.EntityFramework;
using BookShop.Cart.Infrastructure.Inbox;
using BookShop.Shared;
using BookShop.Users.IntegrationEvents;
using BuildingBlocks.Application.EventBus;
using BuildingBlocks.Infrastructure.EntityFramework;
using BuildingBlocks.Presentation.Endpoints;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TickerQ.DependencyInjection;

namespace BookShop.Cart;

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

    private static void AddApplication(this IServiceCollection services)
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
    }

    private static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;
        IConfigurationManager configuration = builder.Configuration;

        builder.AddCustomPostgresDbContext<CartDbContext>(Resources.Postgres, Services.Cart);
        services.AddScoped<ICartDbContext>(provider => provider.GetRequiredService<CartDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CartDbContext>());

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

        services.AddScoped<IIntegrationEventConsumerRepository, IntegrationEventConsumerRepository>();
    }
}
