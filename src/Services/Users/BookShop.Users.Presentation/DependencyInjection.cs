using BookShop.Users.Application;
using BookShop.Users.Infrastructure;
using BuildingBlocks.Presentation.Endpoints;
using Microsoft.Extensions.Hosting;

namespace BookShop.Users.Presentation;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddUsersModule(this IHostApplicationBuilder builder)
    {
        builder.AddApplication();

        builder.AddInfrastructure();

        AddPresentation(builder);

        return builder;
    }

    private static void AddPresentation(IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpoints(typeof(DependencyInjection).Assembly);
    }
}
