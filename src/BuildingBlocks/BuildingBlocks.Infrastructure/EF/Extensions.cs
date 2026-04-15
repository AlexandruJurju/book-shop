using BuildingBlocks.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.EF;

public static class Extensions
{
    public static void AddCustomPostgresDbContext<TDbContext>(
        this IServiceCollection     services,
        IConfiguration              configuration,
        string                      connectionName,
        Action<IServiceCollection>? action = null
    ) where TDbContext : DbContext
    {
        string connectionString = configuration.GetConnectionStringOrThrow(connectionName);

        services.AddDbContext<TDbContext>((sp, options) =>
            {
                options
                    .UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention();

                IInterceptor[] interceptors = sp.GetServices<IInterceptor>()
                    .ToArray();

                if (interceptors.Length != 0)
                {
                    options.AddInterceptors(interceptors);
                }
            }
        );

        action?.Invoke(services);
    }
}
