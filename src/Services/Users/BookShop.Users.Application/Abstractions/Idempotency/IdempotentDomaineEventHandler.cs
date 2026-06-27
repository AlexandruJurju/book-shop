using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Domain;
using Microsoft.Extensions.Logging;

namespace BookShop.Users.Application.Abstractions.Idempotency;

public sealed class IdempotentDomainEventHandler<TDomainEvent>(
    IDomainEventHandler<TDomainEvent> decorated,
    IDomainEventConsumerRepository consumerRepository,
    ILogger<IdempotentDomainEventHandler<TDomainEvent>> logger
) : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public async Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        string consumerName = GetType().Name;

        if (await consumerRepository.ExistsAsync(domainEvent.Id, consumerName, cancellationToken))
        {
            logger.LogInformation("Skipping domain event handler {ConsumerName} for domain event {DomainEventId}", consumerName, domainEvent.Id);
            return;
        }

        await decorated.HandleAsync(domainEvent, cancellationToken);

        await consumerRepository.AddAsync(domainEvent.Id, consumerName, cancellationToken);
    }
}
