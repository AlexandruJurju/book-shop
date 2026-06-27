using BookShop.Users.Application.Abstractions.Idempotency;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Domain;

namespace BookShop.Users.Infrastructure.Outbox;

public abstract class IdempotentDomainEventHandler<TDomainEvent>(
    IDomainEventHandler<TDomainEvent> decorated,
    IDomainEventConsumerRepository consumerRepository
) : DomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public override async Task HandleAsync(TDomainEvent notification, CancellationToken cancellationToken = default)
    {
        string handlerName = GetType().Name;

        if (await consumerRepository.ExistsAsync(notification.Id, handlerName, cancellationToken))
        {
            return;
        }

        await decorated.HandleAsync(notification, cancellationToken);

        await consumerRepository.AddAsync(notification.Id, handlerName, cancellationToken);
    }
}
