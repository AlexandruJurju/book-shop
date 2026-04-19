using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.Data.EntityFramework.Interceptors;

public sealed class SlowQueryInterceptor(
    ILogger<SlowQueryInterceptor> logger
) : DbCommandInterceptor
{
    private const int SlowQueryThresholdMs = 200;

    public override DbDataReader ReaderExecuted(DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result
    )
    {
        if (eventData.Duration.TotalMilliseconds >= SlowQueryThresholdMs)
        {
            logger.LogWarning("Slow Query ({Duration} ms): {Command}", eventData.Duration.TotalMilliseconds, command.CommandText);
        }

        return base.ReaderExecuted(command, eventData, result);
    }

    public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = new()
    )
    {
        if (eventData.Duration.TotalMilliseconds >= SlowQueryThresholdMs)
        {
            logger.LogWarning("Slow Query ({Duration} ms): {Command}", eventData.Duration.TotalMilliseconds, command.CommandText);
        }

        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }
}
