using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Time;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTimeProvider(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
