using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Time;

public static class Extensions
{
    public static IServiceCollection AddTimeProvider(this IServiceCollection services)
    {
        services.AddSingleton<TimeProvider>(TimeProvider.System);
        return services;
    }
}
