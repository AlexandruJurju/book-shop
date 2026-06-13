namespace BookShop.Cart.Application.EventBus;

public interface IIntegrationEventConsumerRepository
{
    Task<bool> InboxConsumerExistsAsync(Guid id, string name);
    Task InsertConsumerAsync(Guid id, string name);
}
