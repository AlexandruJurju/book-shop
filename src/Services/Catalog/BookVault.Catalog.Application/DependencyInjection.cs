using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace BookVault.Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(
            (MediatorOptions options) =>
            {
                options.GenerateTypesAsInternal = true;
                options.Assemblies = [typeof(DependencyInjection).Assembly];
            }
        );

        return services;
    }
}
