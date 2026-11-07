using BookShop.Catalog.Infrastructure.EntityFramework;
using BookShop.Catalog.Infrastructure.Persistence;
using BookShop.Users.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookShop.WebApi.Extensions;

internal static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        ApplyMigration<CatalogDbContext>(scope);
        ApplyMigration<UsersDbContext>(scope);
        // ApplyMigration<CartDbContext>(scope);
    }

    private static void ApplyMigration<TDbContext>(IServiceScope scope)
        where TDbContext : DbContext
    {
        using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        context.Database.Migrate();
    }
}
