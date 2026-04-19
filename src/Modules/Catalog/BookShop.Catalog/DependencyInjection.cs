using BookShop.Catalog.Infrastructure.EntityFramework;
using BookShop.Shared.Aspire;
using BuildingBlocks.AspNetCore.Endpoints;
using BuildingBlocks.Infrastructure.Data;
using BuildingBlocks.Infrastructure.EntityFramework;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Catalog;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddPresentation();

        builder.Services.AddApplication();

        builder.Services.AddInfrastructure(builder.Configuration);

        return builder.Services;
    }

    private static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpoints(typeof(DependencyInjection).Assembly);
        
        return services;
    }

    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomPostgresDbContext<CatalogDbContext>(configuration, Resources.Postgres, Services.Catalog);

        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
