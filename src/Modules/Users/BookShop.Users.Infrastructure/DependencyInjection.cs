using BookShop.Shared.Aspire;
using BookShop.Users.Domain.Users;
using BookShop.Users.Infrastructure.EntityFramework;
using BookShop.Users.Infrastructure.Outbox;
using BookShop.Users.Infrastructure.Users;
using BuildingBlocks.Infrastructure.Cache;
using BuildingBlocks.Infrastructure.Data;
using BuildingBlocks.Infrastructure.EntityFramework;
using BuildingBlocks.Infrastructure.EntityFramework.Interceptors;
using BuildingBlocks.Infrastructure.Outbox;
using BuildingBlocks.Infrastructure.Time;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TickerQ.DependencyInjection;

namespace BookShop.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTimeProvider();

        services.AddCustomMemoryCache(configuration, Services.Users);

        services.AddCustomNpgsql(configuration, Resources.Postgres);

        services.TryAddScoped<IInterceptor, InsertDomainEventsInterceptor>();
        services.AddCustomPostgresDbContext<UsersDbContext>(configuration, Resources.Postgres, Services.Users);

        services.AddScoped<IUserRepository, UserRepository>();

        AddOutboxProcessor(services, configuration);

        return services;
    }

    private static void AddOutboxProcessor(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptionsWithValidateOnStart<OutboxJobOptions>()
            .Bind(configuration.GetSection($"{Services.Users}:{OutboxJobOptions.ConfigurationSection}"))
            .ValidateDataAnnotations();

        OutboxJobOptions outboxJobOptions = configuration
            .GetSection($"{Services.Users}:{OutboxJobOptions.ConfigurationSection}")
            .Get<OutboxJobOptions>()!;

        services.AddTickerQ();

        services.MapTicker<OutboxJob>()
            .WithMaxConcurrency(1)
            .WithCron(outboxJobOptions.Cron);
    }
}
