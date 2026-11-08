namespace BookShop.Users.Application.Abstractions.Idempotency;

public interface IDomainEventConsumerRepository
{
    Task<bool> ExistsAsync(Guid id, string consumerName, CancellationToken cancellationToken);
    Task AddAsync(Guid id, string consumerName, CancellationToken cancellationToken);
}
