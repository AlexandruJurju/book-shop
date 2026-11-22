using BookShop.Cart.Application.Abstractions.Data;
using BookShop.Cart.Infrastructure.Persistence;
using BookShop.Shared;
using BuildingBlocks.Infrastructure.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Cart.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;

        builder.AddCustomPostgresDbContext<CartDbContext>(Resources.Postgres, Services.Cart);
        services.AddScoped<ICartDbContext>(provider => provider.GetRequiredService<CartDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CartDbContext>());
    }
}
