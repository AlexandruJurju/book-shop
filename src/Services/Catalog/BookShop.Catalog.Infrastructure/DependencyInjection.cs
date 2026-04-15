using BookShop.Catalog.Infrastructure.Database;
using BuildingBlocks.Infrastructure.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Constants.Aspire;

namespace BookShop.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomPostgresDbContext<CatalogDbContext>(configuration, CatalogResources.Database);

        return services;
    }
}
