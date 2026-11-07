using BookShop.Catalog.Infrastructure;
using BuildingBlocks.Api.Endpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Catalog.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpoints(typeof(DependencyInjection).Assembly);

        builder.AddInfrastructure();

        return builder.Services;
    }
}
