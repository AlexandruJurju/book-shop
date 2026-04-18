using BookShop.Catalog.Domain.Categories;
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
        modelBuilder.HasDefaultSchema(Schemas.Catalog);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}
