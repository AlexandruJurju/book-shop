using BookShop.Users.Infrastructure;
using BuildingBlocks.AspNetCore.Endpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Users.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpoints(typeof(DependencyInjection).Assembly);

        builder.AddInfrastructure();

        return builder.Services;
    }
}
