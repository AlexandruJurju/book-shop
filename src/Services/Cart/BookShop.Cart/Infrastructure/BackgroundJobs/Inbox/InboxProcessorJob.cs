using Microsoft.Extensions.Logging;
using Quartz;

namespace BookShop.Cart.Infrastructure.BackgroundJobs.Inbox;

internal sealed class InboxProcessorJob(
    InboxProcessor inboxProcessor,
    ILogger<InboxProcessorJob> logger
) : IJob
{
    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Inbox job started");

        await inboxProcessor.ProcessAsync(cancellationToken);

        logger.LogInformation("Inbox job completed");
    }
}
