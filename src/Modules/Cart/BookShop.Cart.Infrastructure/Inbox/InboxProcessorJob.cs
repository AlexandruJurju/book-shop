using Microsoft.Extensions.Logging;
using TickerQ.Exceptions;
using TickerQ.Utilities.Base;
using TickerQ.Utilities.Interfaces;

namespace BookShop.Cart.Infrastructure.Inbox;

internal sealed class InboxProcessorJob(
    InboxProcessor inboxProcessor,
    ILogger<InboxProcessorJob> logger
) : ITickerFunction
{
    public async Task ExecuteAsync(TickerFunctionContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        logger.LogInformation("Inbox job started");

        int processedMessages = await inboxProcessor.ProcessAsync(cancellationToken);

        if (processedMessages == 0)
        {
            throw new TerminateExecutionException("No inbox messages processed");
        }
        
        logger.LogInformation("Inbox job completed");
    }
}
