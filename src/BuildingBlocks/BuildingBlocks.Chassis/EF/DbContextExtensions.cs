using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Chassis.EF;

public static class DbContextExtensions
{
    public static void AddPostgresDbContext<TDbContext>(
        this IHostApplicationBuilder builder,
        string connectionName,
        Action<IServiceCollection>? action = null
    )
        where TDbContext : DbContext
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        builder.AddNpgsqlDbContext<TDbContext>(
            connectionName,
            configureDbContextOptions: options =>
            {
                options.UseSnakeCaseNamingConvention();
            }
        );

        // services.AddDbContext<TDbContext>((serviceProvider, options) =>
        //     {
        //         options.UseNpgsql(configuration.GetRequiredConnectionString(connectionName));
        //         options.UseSnakeCaseNamingConvention();
        //
        //         var interceptors = serviceProvider.GetServices<IInterceptor>().ToArray();
        //         if (interceptors.Length != 0)
        //         {
        //             options.AddInterceptors(interceptors);
        //         }
        //     }
        // );

        action?.Invoke(services);
    }
}
