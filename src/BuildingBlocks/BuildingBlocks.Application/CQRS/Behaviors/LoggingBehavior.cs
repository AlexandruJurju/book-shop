using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace BuildingBlocks.Application.CQRS.Behaviors;

public sealed partial class LoggingBehavior<TMessage, TResponse>(
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
        string requestName = message.GetType().Name;
        string moduleName = GetModuleName(typeof(TMessage).FullName!);

        using (LogContext.PushProperty("Module", moduleName))
        {
            LogExecutingRequest(requestName);

            TResponse result = await next(message, cancellationToken);

            if (result.IsSuccess)
            {
                LogCompletedRequest(requestName);
            }
            else
            {
                using (LogContext.PushProperty("Errors", result.Errors, true))
                {
                    LogFailedRequest(requestName);
                }
            }

            return result;
        }
    }

    [LoggerMessage(LogLevel.Information, "Executing request {RequestName}")]
    partial void LogExecutingRequest(string requestName);

    [LoggerMessage(LogLevel.Information, "Completed request {RequestName}")]
    partial void LogCompletedRequest(string requestName);

    [LoggerMessage(LogLevel.Error, "Request {RequestName} processed with errors")]
    partial void LogFailedRequest(string requestName);

    private static string GetModuleName(string requestName)
    {
        return requestName.Split('.')[1];
    }
}
