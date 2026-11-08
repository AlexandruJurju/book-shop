using Microsoft.Extensions.Logging;
using Quartz;

namespace BookShop.Users.Infrastructure.BackgroundJobs.Outbox;

internal sealed class OutboxProcessorJob(
    OutboxProcessor outboxProcessor,
    ILogger<OutboxProcessorJob> logger
) : IJob
{
    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Outbox job started");

        await outboxProcessor.ProcessAsync(cancellationToken);

        logger.LogInformation("Outbox job completed");
    }
}
