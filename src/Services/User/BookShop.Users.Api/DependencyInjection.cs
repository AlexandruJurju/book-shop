using BookShop.Users.Application;
using BookShop.Users.Infrastructure;
using BuildingBlocks.Api.Endpoints;
using Microsoft.Extensions.Hosting;

namespace BookShop.Users.Api;

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
