namespace BookShop.Users.Application.Abstractions.Idempotency;

public interface IDomainEventConsumerRepository
{
    Task<bool> ExistsAsync(Guid outboxMessageId, string handlerName, CancellationToken cancellationToken);
    Task AddAsync(Guid outboxMessageId, string handlerName, CancellationToken cancellationToken);
}
