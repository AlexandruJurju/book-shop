using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace BuildingBlocks.Infrastructure.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomNpgsql(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionName
    )
    {
        string connectionString = configuration.GetConnectionStringOrThrow(connectionName);

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(connectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);
        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

        return services;
    }
}
