using BookShop.Cart.Application.Abstractions.Data;
using BookShop.Shared;
using BuildingBlocks.Infrastructure.Inbox;
using BuildingBlocks.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Cart.Infrastructure.EntityFramework;

public sealed class CartDbContext(
    DbContextOptions<CartDbContext> options
) : DbContext(options), IUnitOfWork, ICartDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Services.Cart);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartDbContext).Assembly);

        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());

        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
    }
}
