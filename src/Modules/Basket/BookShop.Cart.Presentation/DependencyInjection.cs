using BuildingBlocks.AspNetCore.Endpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Cart.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddCartModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpoints(typeof(DependencyInjection).Assembly);

        return builder.Services;
    }
}
