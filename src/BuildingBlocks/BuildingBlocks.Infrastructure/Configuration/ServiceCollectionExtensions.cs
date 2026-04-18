using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration;

public static class ServiceCollectionExtensions
{
    public static string GetConnectionStringOrThrow(this IConfiguration configuration, string name)
    {
        return configuration.GetConnectionString(name) ??
            throw new InvalidOperationException($"The connection string {name} was not found");
    }
}
