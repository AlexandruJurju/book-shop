using BookShop.Users.Infrastructure.EntityFramework;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Outbox;
using Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookShop.Users.Infrastructure.Outbox;

internal sealed class OutboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IPublisher publisher,
    IOptions<OutboxJobOptions> outboxOptions,
    TimeProvider timeProvider,
    ILogger<OutboxJob> logger
) : OutboxJobBase(dbConnectionFactory, publisher, outboxOptions, timeProvider, logger)
{
    protected override string ModuleName => "Users";
    protected override string SchemaName => Schemas.Users;
}
