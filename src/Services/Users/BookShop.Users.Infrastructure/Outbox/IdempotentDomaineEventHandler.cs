using BookShop.Users.Application.Abstractions.Idempotency;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Domain;

namespace BookShop.Users.Infrastructure.Outbox;

public sealed class IdempotentDomainEventHandler<TDomainEvent>(
    IDomainEventHandler<TDomainEvent> decorated,
    IDomainEventConsumerRepository consumerRepository
) : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public async Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        string handlerName = GetType().Name;

        if (await consumerRepository.ExistsAsync(domainEvent.Id, handlerName, cancellationToken))
        {
            return;
        }

        await decorated.HandleAsync(domainEvent, cancellationToken);

        await consumerRepository.AddAsync(domainEvent.Id, handlerName, cancellationToken);
    }
}
