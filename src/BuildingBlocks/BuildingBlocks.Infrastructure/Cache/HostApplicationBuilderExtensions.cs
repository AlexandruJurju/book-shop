using BookShop.Shared;
using BuildingBlocks.Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Infrastructure.Cache;

public static class HostApplicationBuilderExtensions
{
    public static IServiceCollection AddCustomCache(
        this IHostApplicationBuilder builder
    )
    {
        IServiceCollection services = builder.Services;
        IConfiguration configuration = builder.Configuration;

        services.ConfigureWithValidation<CachingOptions>(CachingOptions.SectionName);

        CachingOptions cachingOptions = configuration
            .GetRequiredSection(CachingOptions.SectionName)
            .Get<CachingOptions>()!;
        
        builder.AddRedisDistributedCache(Resources.Redis);

        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = cachingOptions.MaximumPayloadBytes;

            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(cachingOptions.LocalExpirationInMinutes),
                Expiration = TimeSpan.FromMinutes(cachingOptions.DistributedExpirationInMinutes)
            };
        });

        return services;
    }
}
