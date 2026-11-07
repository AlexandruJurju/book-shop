using System.Reflection;
using BookShop.Shared;
using BuildingBlocks.Api.OpenApi;
using BuildingBlocks.Application.CQRS.Behaviors;
using BuildingBlocks.Infrastructure.Authentication;
using BuildingBlocks.Infrastructure.Authorization;
using BuildingBlocks.Infrastructure.Cache;
using BuildingBlocks.Infrastructure.Data;
using BuildingBlocks.Infrastructure.Data.Postgres;
using BuildingBlocks.Infrastructure.EntityFramework.Interceptors;
using BuildingBlocks.Infrastructure.EventBus;
using BuildingBlocks.Infrastructure.Inbox;
using FluentValidation;
using MassTransit;
using Mediator;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;

namespace BookShop.WebApi;

internal static class DependencyInjection
{
    public static void AddApplication(this IHostApplicationBuilder builder, Assembly[] moduleAssemblies)
    {
        IServiceCollection services = builder.Services;

        builder.Services.AddMediator((MediatorOptions options) =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssemblies(moduleAssemblies, includeInternalTypes: true);
    }


    public static void AddPresentation(this IHostApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;

        services.AddCustomOpenApi();
    }

    public static void AddInfrastructure(
        this IHostApplicationBuilder builder,
        Action<IRegistrationConfigurator, string>[] moduleConfigureConsumers
    )
    {
        IServiceCollection services = builder.Services;
        IConfiguration configuration = builder.Configuration;

        builder.Services.AddTransient<IIntegrationEventsDispatcher, IntegrationEventsDispatcher>();

        services.AddCustomAuthentication();

        services.AddCustomAuthorization();

        builder.AddCustomCacheInMemory();

        services.AddCustomMassTransitInMemory(moduleConfigureConsumers);

        services.AddCustomPostgres(configuration, Resources.Postgres);
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
