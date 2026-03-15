using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Chassis.Endpoints;

public static class RegisterEndpointsExtension
{
    public static void AddEndpoints(this IServiceCollection services, Type type)
    {
        services.Scan(scan =>
            scan.FromAssembliesOf(type)
                .AddClasses(classes => classes
                    .AssignableTo<IEndpoint>()
                    .Where(typeInfo => typeInfo is { IsAbstract: false, IsInterface: false })
                )
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );
    }
}