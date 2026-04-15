using BookShop.Catalog.Domain.Categories;
using BuildingBlocks.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Catalog.Infrastructure.Database;

public sealed class CatalogDbContext(
    DbContextOptions<CatalogDbContext> options
) : DbContext(options), IUnitOfWork
{
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}
