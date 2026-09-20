using BuildingBlocks.Application.Abstractions.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Authentication;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomAuthentication(
        this IServiceCollection services
    )
    {
        services.ConfigureOptions<JwtBearerConfigureOptions>();

        services.AddAuthentication()
            .AddJwtBearer();

        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}
