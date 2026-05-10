namespace BookShop.Users.Application.Abstractions.Idempotency;

public interface IIdempotencyDomainEventRepository
{
    Task<bool> ExistsAsync(Guid outboxMessageid, string handlerName, CancellationToken cancellationToken);
    Task AddAsync(Guid outboxMessageid, string handlerName, CancellationToken cancellationToken);
}
