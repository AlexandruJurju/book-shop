using BuildingBlocks.Application.EventBus;

namespace BookShop.Cart.Application.EventBus;

internal sealed class IdempotentIntegrationEventHandler<TIntegrationEvent>(
    IIntegrationEventHandler<TIntegrationEvent> decorated,
    IIntegrationEventConsumerRepository consumerRepository
) : IntegrationEventHandler<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
    public override async Task Handle(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        string consumerName = decorated.GetType().Name;

        if (await consumerRepository.InboxConsumerExistsAsync(integrationEvent.Id, consumerName))
        {
            return;
        }

        await decorated.Handle(integrationEvent, cancellationToken);

        await consumerRepository.InsertConsumerAsync(integrationEvent.Id, consumerName);
    }
}
