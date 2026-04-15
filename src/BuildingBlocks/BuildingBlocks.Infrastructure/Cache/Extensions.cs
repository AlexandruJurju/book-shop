using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Cache;

// todo: add distributed cache
public static class Extensions
{
    public static IServiceCollection AddCache(
        this IServiceCollection services,
        IConfiguration          configuration
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

            options.DefaultEntryOptions = new()
            {
                Expiration = cacheOptions.Expiration,
                LocalCacheExpiration = cacheOptions.Expiration,
            };
        });

        return services;
    }
}
