namespace BookShop.Cart.Application.EventBus;

public interface IIntegrationEventConsumerRepository
{
    Task<bool> ExistsAsync(Guid id, string consumerName);
    Task AddAsync(Guid id, string consumerName);
}
