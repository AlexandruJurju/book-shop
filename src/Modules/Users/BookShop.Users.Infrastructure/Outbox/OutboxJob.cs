using BookShop.Shared.Aspire;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Outbox;
using Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookShop.Users.Infrastructure.Outbox;

internal sealed class OutboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IPublisher publisher,
    IOptionsMonitor<OutboxJobOptions> outboxOptions,
    TimeProvider timeProvider,
    ILogger logger
) : OutboxJobBase(dbConnectionFactory, publisher, outboxOptions, timeProvider, logger)
{
    protected override string ServiceName => Services.Users;
    protected override string SchemaName => Services.Users;
}
