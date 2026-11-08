using BookShop.Cart.Application.EventBus;
using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.Logging;

namespace BookShop.Cart.Application.Abstractions;

internal sealed class IdempotentIntegrationEventHandler<TIntegrationEvent>(
    IIntegrationEventHandler<TIntegrationEvent> decorated,
    IIntegrationEventConsumerRepository consumerRepository,
    ILogger<IdempotentIntegrationEventHandler<TIntegrationEvent>> logger) : IIntegrationEventHandler<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
    public async Task HandleAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        string consumerName = decorated.GetType().Name;

        if (await consumerRepository.ExistsAsync(integrationEvent.Id, consumerName))
        {
            logger.LogInformation("Skipping integration event handler {ConsumerName} for integration event {IntegrationEventId}", consumerName, integrationEvent.Id);
            return;
        }

        await decorated.HandleAsync(integrationEvent, cancellationToken);

        await consumerRepository.AddAsync(integrationEvent.Id, consumerName);
    }
}
