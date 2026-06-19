using BookShop.Catalog.Application.Abstractions.Data;
using BookShop.Catalog.Domain.Categories;
using BookShop.Shared;
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
