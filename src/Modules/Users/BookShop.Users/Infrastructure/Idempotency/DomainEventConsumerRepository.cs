using BookShop.Users.Application.Abstractions.Idempotency;
using BookShop.Users.Infrastructure.EntityFramework;
using BuildingBlocks.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Users.Infrastructure.Idempotency;

internal sealed class DomainEventConsumerRepository(
    UsersDbContext dbContext
) : IDomainEventConsumerRepository
{
    public Task<bool> ExistsAsync(Guid outboxMessageId, string handlerName, CancellationToken cancellationToken)
        => dbContext.Set<OutboxMessageConsumer>()
            .AnyAsync(e => e.OutboxMessageId == outboxMessageId && e.Name == handlerName, cancellationToken);

    public async Task AddAsync(Guid outboxMessageId, string handlerName, CancellationToken cancellationToken)
    {
        dbContext.Set<OutboxMessageConsumer>().Add(new OutboxMessageConsumer(outboxMessageId, handlerName));
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
