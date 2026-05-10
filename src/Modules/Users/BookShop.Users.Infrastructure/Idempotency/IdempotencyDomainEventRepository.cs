using BookShop.Users.Application.Abstractions.Idempotency;
using BookShop.Users.Infrastructure.EntityFramework;
using BuildingBlocks.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Users.Infrastructure.Idempotency;

internal sealed class IdempotencyDomainEventRepository(
    UsersDbContext dbContext
) : IIdempotencyDomainEventRepository
{
    public Task<bool> ExistsAsync(Guid outboxMessageid, string handlerName, CancellationToken cancellationToken)
        => dbContext.Set<OutboxMessageConsumer>()
            .AnyAsync(e => e.OutboxMessageId == outboxMessageid && e.Name == handlerName, cancellationToken);

    public async Task AddAsync(Guid outboxMessageid, string handlerName, CancellationToken cancellationToken)
    {
        dbContext.Set<OutboxMessageConsumer>().Add(new OutboxMessageConsumer(outboxMessageid, handlerName));
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
