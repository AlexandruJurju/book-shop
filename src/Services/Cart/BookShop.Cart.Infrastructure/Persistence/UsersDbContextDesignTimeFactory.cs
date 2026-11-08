using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookShop.Cart.Infrastructure.Persistence;

public class CartDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<CartDbContext>
{
    public CartDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CartDbContext>();

        optionsBuilder.UseNpgsql()
            .UseSnakeCaseNamingConvention();

        return new CartDbContext(optionsBuilder.Options);
    }
}
