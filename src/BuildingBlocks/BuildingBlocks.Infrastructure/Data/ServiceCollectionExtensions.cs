using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace BuildingBlocks.Infrastructure.Data;

public static class ServiceCollectionExtensions
{
    public static void AddCustomPostgresDbContext<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionName,
        string service,
        Action<IServiceCollection>? action = null
    ) where TDbContext : DbContext, IUnitOfWork
    {
        string connectionString = configuration.GetConnectionStringOrThrow(connectionName);

        services.AddDbContext<TDbContext>((sp, options) =>
            {
                options
                    .UseNpgsql(
                        connectionString,
                        npgsqlOptions =>
                            npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, service)
                    )
                    .UseSnakeCaseNamingConvention();

                IInterceptor[] interceptors = sp.GetServices<IInterceptor>()
                    .ToArray();

                if (interceptors.Length != 0)
                {
                    options.AddInterceptors(interceptors);
                }
            }
        );

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TDbContext>());

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(connectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);
        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

        action?.Invoke(services);
    }
}
