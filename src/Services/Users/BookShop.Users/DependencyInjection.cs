using BookShop.Shared;
using BookShop.Users.Application.Abstractions.CQRS;
using BookShop.Users.Application.Abstractions.Data;
using BookShop.Users.Application.Abstractions.Idempotency;
using BookShop.Users.Application.Abstractions.Identity;
using BookShop.Users.Infrastructure.Authorization;
using BookShop.Users.Infrastructure.EntityFramework;
using BookShop.Users.Infrastructure.Idempotency;
using BookShop.Users.Infrastructure.IdentityProvider;
using BookShop.Users.Infrastructure.Outbox;
using BuildingBlocks.Application.Authorization;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.EntityFramework;
using BuildingBlocks.Infrastructure.Keycloak;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TickerQ.DependencyInjection;

namespace BookShop.Users;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddUsersModule(this IHostApplicationBuilder builder)
    {
        AddInfrastructure(builder);

        AddPresentation(builder);

        AddApplication(builder);

        return builder;
    }

    private static void AddApplication(IHostApplicationBuilder builder)
    {
        builder.Services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        builder.Services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();

        builder.Services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }

    private static void AddPresentation(IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpoints(typeof(DependencyInjection).Assembly);
    }

    private static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;
        IConfigurationManager configuration = builder.Configuration;

        services.AddScoped<IDomainEventConsumerRepository, DomainEventConsumerRepository>();

        builder.AddCustomPostgresDbContext<UsersDbContext>(Resources.Postgres, Services.Users);
        services.AddScoped<IUsersDbContext>(provider => provider.GetRequiredService<UsersDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());

        services.AddScoped<IPermissionService, PermissionService>();

        AddOutboxJob(services, configuration);

        AddKeycloakIdentityProvider(services);
    }

    private static void AddKeycloakIdentityProvider(IServiceCollection services)
    {
        services.ConfigureWithValidation<KeycloakOptions>(KeycloakOptions.SectionName);

        services.AddTransient<KeycloakAuthDelegatingHandler>();

        services.AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
            {
                KeycloakOptions keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<KeycloakAuthDelegatingHandler>();

        services.AddTransient<IIdentityProviderService, KeycloakIdentityProviderService>();
    }

    private static void AddOutboxJob(IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection section = configuration
            .GetRequiredSection($"Jobs:{OutboxJobOptions.ConfigurationSection}");

        services
            .AddOptionsWithValidateOnStart<OutboxJobOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<OutboxProcessor>();
        services.AddScoped<OutboxProcessorJob>();

        OutboxJobOptions outboxJobOptions = section.Get<OutboxJobOptions>()!;

        services
            .MapTicker<OutboxProcessorJob>()
            .WithMaxConcurrency(1)
            .WithCron(outboxJobOptions.Cron);
    }
}
