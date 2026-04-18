using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TickerQ.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Outbox;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOutbox<TOutboxJob>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TOutboxJob : OutboxJobBase
    {
        services
            .AddOptionsWithValidateOnStart<OutboxJobOptions>()
            .Bind(configuration.GetSection(OutboxJobOptions.ConfigurationSection))
            .ValidateDataAnnotations();

        OutboxJobOptions outboxJobOptions = configuration
            .GetSection(OutboxJobOptions.ConfigurationSection)
            .Get<OutboxJobOptions>()!;

        services.AddTickerQ();
        
        services.MapTicker<TOutboxJob>()
            .WithMaxConcurrency(1)
            .WithCron(outboxJobOptions.Cron);

        return services;
    }
}
