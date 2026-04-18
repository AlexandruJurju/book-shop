using BookShop.Basket.Application;
using BookShop.Basket.Infrastructure;
using BuildingBlocks.AspNetCore.Endpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Basket.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddBasketModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpoints(typeof(DependencyInjection).Assembly);

        builder.Services.AddApplication();

        builder.Services.AddInfrastructure(builder.Configuration);

        return builder.Services;
    }
}
