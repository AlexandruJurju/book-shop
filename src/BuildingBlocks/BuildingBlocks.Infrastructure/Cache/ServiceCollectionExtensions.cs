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
        IConfiguration configuration
    )
    {
        services
            .AddOptionsWithValidateOnStart<CacheOptions>()
            .Bind(configuration.GetSection(CacheOptions.ConfigurationSection))
            .ValidateDataAnnotations();

        CacheOptions cacheOptions = configuration
            .GetSection(CacheOptions.ConfigurationSection)
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

    public static IServiceCollection AddCustomDistributedCache(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionStringOrThrow(Resources.Redis);
        });

        services
            .AddOptionsWithValidateOnStart<CacheOptions>()
            .Bind(configuration.GetSection(CacheOptions.ConfigurationSection))
            .ValidateDataAnnotations();

        CacheOptions cacheOptions = configuration
            .GetSection(CacheOptions.ConfigurationSection)
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
