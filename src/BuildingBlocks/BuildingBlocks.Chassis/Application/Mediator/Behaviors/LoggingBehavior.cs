using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace BuildingBlocks.Chassis.Application.Mediator.Behaviors;

public sealed class LoggingBehavior<TMessage, TResponse>(
    ILogger<LoggingBehavior<TMessage, TResponse>> logger
) : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : Result
{
    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var requestName = message.GetType().Name;

        try
        {
            logger.LogInformation("Executing request {RequestName}", requestName);

            var result = await next(message, cancellationToken);

            if (result.IsSuccess)
            {
                logger.LogInformation("Successfully executed {RequestName}", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Errors", result.Errors, true))
                {
                    logger.LogError("Request {RequestName} processed with error", requestName);
                }
            }

            return result;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Request {RequestName} processing failed", requestName);
            throw;
        }
    }
}
