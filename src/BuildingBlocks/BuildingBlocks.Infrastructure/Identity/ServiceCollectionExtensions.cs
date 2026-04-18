using BuildingBlocks.Application.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Identity;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        return services;
    }
}
