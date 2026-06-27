using Microsoft.Extensions.Logging;
using TickerQ.Exceptions;
using TickerQ.Utilities.Base;
using TickerQ.Utilities.Interfaces;

namespace BookShop.Users.Infrastructure.Outbox;

internal sealed class OutboxProcessorJob(
    OutboxProcessor outboxProcessor,
    ILogger<OutboxProcessorJob> logger
) : ITickerFunction
{
    public async Task ExecuteAsync(TickerFunctionContext context, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Outbox job started");

        int processedMessages = await outboxProcessor.ProcessAsync(cancellationToken);

        if (processedMessages == 0)
        {
            throw new TerminateExecutionException("No outbox messages processed");
        }

        logger.LogInformation("Outbox job completed");
    }
}
