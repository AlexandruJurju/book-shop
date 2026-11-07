using BookShop.Catalog.Application.Abstractions;
using BookShop.Catalog.Infrastructure.EntityFramework;
using BookShop.Catalog.Infrastructure.Persistence;
using BookShop.Shared;
using BuildingBlocks.Infrastructure.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        IServiceCollection services = new ServiceCollection();

        builder.AddCustomPostgresDbContext<CatalogDbContext>(Resources.Postgres, Services.Catalog);
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CatalogDbContext>());
    }
}
