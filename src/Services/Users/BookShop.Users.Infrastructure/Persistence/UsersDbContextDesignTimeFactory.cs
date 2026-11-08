using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookShop.Users.Infrastructure.Persistence;

public class UsersDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
{
    public UsersDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();

        optionsBuilder.UseNpgsql()
            .UseSnakeCaseNamingConvention();

        return new UsersDbContext(optionsBuilder.Options);
    }
}
