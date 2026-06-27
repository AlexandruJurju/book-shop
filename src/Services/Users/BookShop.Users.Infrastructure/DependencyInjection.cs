using BookShop.Shared;
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
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.EntityFramework;
using BuildingBlocks.Infrastructure.Keycloak;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TickerQ.DependencyInjection;

namespace BookShop.Users.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;
        IConfigurationManager configuration = builder.Configuration;

        builder.AddCustomPostgresDbContext<UsersDbContext>(Resources.Postgres, Services.Users);
        services.AddScoped<IUsersDbContext>(provider => provider.GetRequiredService<UsersDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());

        services.AddScoped<IDomainEventConsumerRepository, DomainEventConsumerRepository>();
        AddOutboxJob(services, configuration);

        services.AddScoped<IPermissionService, PermissionService>();
        AddKeycloakIdentityProvider(services);

        return builder;
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
