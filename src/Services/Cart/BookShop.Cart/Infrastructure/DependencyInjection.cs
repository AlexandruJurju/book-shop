using BookShop.Cart.Application.Abstractions.Data;
using BookShop.Cart.Application.EventBus;
using BookShop.Cart.Infrastructure.BackgroundJobs.Inbox;
using BookShop.Cart.Infrastructure.Persistence;
using BookShop.Shared;
using BuildingBlocks.Infrastructure.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace BookShop.Cart.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;
        IConfigurationManager configuration = builder.Configuration;

        builder.AddCustomPostgresDbContext<CartDbContext>(Resources.Postgres, Services.Cart);
        services.AddScoped<ICartDbContext>(provider => provider.GetRequiredService<CartDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CartDbContext>());

        services.AddScoped<IIntegrationEventConsumerRepository, IntegrationEventConsumerRepository>();
        AddInboxJob(services, configuration);
    }

    private static void AddInboxJob(IServiceCollection services, IConfigurationManager configuration)
    {
        IConfigurationSection section = configuration
            .GetRequiredSection($"Jobs:{InboxJobOptions.ConfigurationSection}");

        services
            .AddOptionsWithValidateOnStart<InboxJobOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<InboxProcessor>();
        services.AddScoped<InboxProcessorJob>();

        InboxJobOptions inboxJobOptions = section.Get<InboxJobOptions>()!;

        services.AddQuartz(options =>
        {
            options.AddJob<InboxProcessorJob>(job =>
                job.WithIdentity(nameof(InboxProcessorJob)));

            options.AddTrigger(trigger => trigger
                .ForJob(nameof(InboxProcessorJob))
                .WithIdentity($"{nameof(InboxProcessorJob)}-trigger")
                .WithCronSchedule(inboxJobOptions.Cron));
        });
    }
}
