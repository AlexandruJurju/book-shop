using BookShop.Shared;
using BookShop.Users.Application.Abstractions.Data;
using BookShop.Users.Domain.Users;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Users.Infrastructure.EntityFramework;

public sealed class UsersDbContext(
    DbContextOptions<UsersDbContext> options
) : DbContext(options), IUnitOfWork, IUsersDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Services.Users);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
    }
}
