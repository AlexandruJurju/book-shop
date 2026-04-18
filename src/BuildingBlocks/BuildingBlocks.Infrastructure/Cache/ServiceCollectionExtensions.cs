using BookShop.Shared.Aspire;
using BuildingBlocks.Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Cache;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomMemoryCache(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName
    )
    {
        services
            .AddOptionsWithValidateOnStart<CacheOptions>(serviceName)
            .Bind(configuration.GetSection($"{serviceName}:{CacheOptions.ConfigurationSection}"))
            .ValidateDataAnnotations();

        CacheOptions cacheOptions = configuration
            .GetSection($"{serviceName}:{CacheOptions.ConfigurationSection}")
            .Get<CacheOptions>()!;

        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = cacheOptions.MaximumPayloadBytes;

            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(cacheOptions.LocalExpirationInMinutes),
                Expiration = TimeSpan.FromMinutes(cacheOptions.DistributedExpirationInMinutes)
            };
        });

        return services;
    }
}
