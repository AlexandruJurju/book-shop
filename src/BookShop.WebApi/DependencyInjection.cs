using System.Reflection;
using BookShop.Catalog.Api;
using BuildingBlocks.Application.Mediator.Behaviors;
using Mediator;

namespace BookShop.WebApi;

internal static class DependencyInjection
{
    public static void AddModules(this WebApplicationBuilder builder, List<Assembly> assemblies)
    {
        builder.AddCatalogModule();
        builder.Services.AddModularMediator(assemblies);
    }

    private static void AddModularMediator(this IServiceCollection services, List<Assembly> assemblies)
    {
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
            options.Assemblies = [..assemblies];
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }
}
