using BookShop.Catalog.Domain.Categories;
using BookShop.Shared.Aspire;
using BuildingBlocks.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Catalog.Infrastructure.EntityFramework;

public sealed class CatalogDbContext(
    DbContextOptions<CatalogDbContext> options
) : DbContext(options), IUnitOfWork
{
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Services.Catalog);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}
