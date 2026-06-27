using BookShop.Cart.Application.EventBus;
using BuildingBlocks.Application.EventBus;

namespace BookShop.Cart.Infrastructure.Inbox;

internal sealed class IdempotentIntegrationEventHandler<TIntegrationEvent>(
    IIntegrationEventHandler<TIntegrationEvent> decorated,
    IIntegrationEventConsumerRepository consumerRepository
) : IIntegrationEventHandler<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
    public async Task HandleAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        string consumerName = decorated.GetType().Name;

        if (await consumerRepository.ExistsAsync(integrationEvent.Id, consumerName))
        {
            return;
        }

        await decorated.HandleAsync(integrationEvent, cancellationToken);

        await consumerRepository.AddAsync(integrationEvent.Id, consumerName);
    }
}
