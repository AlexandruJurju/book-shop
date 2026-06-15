using BookShop.Users.Application.Abstractions.Idempotency;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Domain;

namespace BookShop.Users.Application.Abstractions.CQRS;

internal sealed class IdempotentDomainEventHandler<TDomainEvent>(
    IDomainEventHandler<TDomainEvent> decorated,
    IDomainEventConsumerRepository consumerRepository
) : DomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public override async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        string handlerName = decorated.GetType().Name; 
        
        if (await consumerRepository.ExistsAsync(domainEvent.Id, handlerName, cancellationToken))
        {
            return;
        }

        await decorated.Handle(domainEvent, cancellationToken);

        await consumerRepository.AddAsync(domainEvent.Id, handlerName, cancellationToken);
    }
}
