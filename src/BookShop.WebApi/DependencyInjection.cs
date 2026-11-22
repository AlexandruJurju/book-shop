using System.Reflection;
using BookShop.Shared;
using BuildingBlocks.Api.OpenApi;
using BuildingBlocks.Application.CQRS.Behaviors;
using BuildingBlocks.Infrastructure.Authentication;
using BuildingBlocks.Infrastructure.Authorization;
using BuildingBlocks.Infrastructure.Cache;
using BuildingBlocks.Infrastructure.Data.Postgres;
using BuildingBlocks.Infrastructure.EntityFramework.Interceptors;
using BuildingBlocks.Infrastructure.EventBus;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;

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

    public static void AddInfrastructure(this IHostApplicationBuilder builder, Assembly[] moduleAssemblies)
    {
        IServiceCollection services = builder.Services;
        IConfiguration configuration = builder.Configuration;

        services.AddCustomAuthentication();
        services.AddCustomAuthorization();

        builder.AddCustomCacheInMemory();

        services.AddCustomWolverineRabbitMq(configuration, moduleAssemblies, Resources.RabbitMq);

        services.AddCustomPostgres(configuration, Resources.Postgres);
        services.AddScoped<IInterceptor, InsertOutboxMessagesInterceptor>();
        services.AddScoped<IInterceptor, SoftDeleteInterceptor>();
        services.AddScoped<IInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<IInterceptor, SlowQueryInterceptor>();

        builder.AddQuartz();
        builder.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        builder.Services.AddQuartzHttpApi(options =>
        {
            options.ApiPath = "/quartz-api";
        });
        builder.Services.AddQuartzDashboard(options =>
        {
            options.DashboardPath = "/jobs";
        });

        services.TryAddSingleton(TimeProvider.System);
    }
}
