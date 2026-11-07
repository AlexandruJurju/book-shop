using BookShop.Catalog.Application.Abstractions;
using BookShop.Catalog.Domain.Authors;
using BookShop.Catalog.Domain.Books;
using BookShop.Shared;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Catalog.Infrastructure.EntityFramework;

public sealed class CatalogDbContext(
    DbContextOptions<CatalogDbContext> options
) : DbContext(options), IUnitOfWork
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Services.Catalog);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}
