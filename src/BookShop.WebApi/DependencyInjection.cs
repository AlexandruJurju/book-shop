using System.Reflection;
using BookShop.Shared;
using BuildingBlocks.Application.CQRS.Behaviors;
using BuildingBlocks.Application.EventBus;
using BuildingBlocks.AspNetCore.OpenApi;
using BuildingBlocks.Infrastructure.Authentication;
using BuildingBlocks.Infrastructure.Authorization;
using BuildingBlocks.Infrastructure.Cache;
using BuildingBlocks.Infrastructure.Data;
using BuildingBlocks.Infrastructure.EntityFramework.Interceptors;
using BuildingBlocks.Infrastructure.EventBus;
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
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddCustomOpenApi();
    }

    public static void AddApplication(this IServiceCollection services, Assembly[] moduleAssemblies)
    {
        services.AddMediator((MediatorOptions options) =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddValidatorsFromAssemblies(moduleAssemblies, includeInternalTypes: true);
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

        builder.AddCustomCache();

        services.AddCustomMassTransit(moduleConfigureConsumers);

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
            });
        });

        services.TryAddSingleton(TimeProvider.System);
    }
}
