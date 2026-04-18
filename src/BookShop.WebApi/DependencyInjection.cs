using BookShop.Catalog;
using BookShop.Users.Presentation;
using BuildingBlocks.Application.CQRS.Behaviors;
using Mediator;

namespace BookShop.WebApi;

internal static class DependencyInjection
{
    public static void AddModules(this WebApplicationBuilder builder)
    {
        builder.AddCatalogModule();
        builder.AddUsersModule();
    }

    public static void AddModuleMediator(this IServiceCollection services)
    {
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }
}
