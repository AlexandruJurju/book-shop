using BuildingBlocks.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Infrastructure.EntityFramework;

public static class HostApplicationBuilderExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        public void AddCustomPostgresDbContext<TDbContext>(
            string postgresResourceName,
            string schemaName
        ) where TDbContext : DbContext
        {
            IServiceCollection services = builder.Services;
            IConfigurationManager configuration = builder.Configuration;
            string connectionString = configuration.GetRequiredConnectionString(postgresResourceName);

            services.AddDbContext<TDbContext>((sp, options) =>
                {
                    options
                        .UseNpgsql(
                            connectionString,
                            npgsqlOptions =>
                                npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schemaName)
                        )
                        .UseSnakeCaseNamingConvention();

                    IInterceptor[] interceptors = sp.GetServices<IInterceptor>().ToArray();
                    if (interceptors.Length != 0)
                    {
                        options.AddInterceptors(interceptors);
                    }
                }
            );

            builder.EnrichNpgsqlDbContext<TDbContext>(configureSettings =>
            {
                configureSettings.CommandTimeout = 30;
            });
        }
    }
}
