using BookShop.Shared;
using BuildingBlocks.Presentation.OpenApi;
using BuildingBlocks.Infrastructure.Authentication;
using BuildingBlocks.Infrastructure.Authorization;
using BuildingBlocks.Infrastructure.Cache;
using BuildingBlocks.Infrastructure.Data;
using BuildingBlocks.Infrastructure.EntityFramework.Interceptors;
using BuildingBlocks.Infrastructure.EventBus;
using MassTransit;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;

namespace BookShop.WebApi;

internal static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddCustomOpenApi();
    }

    public static void AddInfrastructure(
        this IHostApplicationBuilder builder,
        Action<IRegistrationConfigurator, string>[] moduleConfigureConsumers
    )
    {
        IServiceCollection services = builder.Services;
        IConfiguration configuration = builder.Configuration;

        services.AddCustomAuthentication();

        services.AddCustomAuthorization();

        builder.AddCustomCacheInMemory();

        services.AddCustomMassTransitInMemory(moduleConfigureConsumers);

        services.AddCustomNpgsql(configuration, Resources.Postgres);
        services.AddScoped<IInterceptor, InsertOutboxMessagesInterceptor>();
        services.AddScoped<IInterceptor, SoftDeleteInterceptor>();
        services.AddScoped<IInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<IInterceptor, SlowQueryInterceptor>();

        services.AddTickerQ(opt =>
        {
            opt.AddDashboard(dashboard =>
            {
                dashboard.SetBasePath("/management/jobs");
                dashboard.WithNoAuth();
            });
        });

        services.TryAddSingleton(TimeProvider.System);
    }
}
