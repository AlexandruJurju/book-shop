using System.Reflection;
using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Wolverine;
using Wolverine.RabbitMQ;

namespace BuildingBlocks.Infrastructure.EventBus;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomWolverineRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly[] moduleAssemblies,
        string rabbitMqResource
    )
    {
        services.TryAddScoped<IEventBus, EventBus>();

        string connectionString = configuration.GetConnectionString(rabbitMqResource)
                                  ?? throw new InvalidOperationException(
                                      $"RabbitMQ connection string '{rabbitMqResource}' was not found."
                                  );

        services.AddWolverine(options =>
        {
            options.UseRuntimeCompilation();

            options.UseRabbitMq(new Uri(connectionString))
                .AutoProvision();

            foreach (Assembly assembly in moduleAssemblies)
            {
                options.Discovery.IncludeAssembly(assembly);
            }
        });

        return services;
    }
}
