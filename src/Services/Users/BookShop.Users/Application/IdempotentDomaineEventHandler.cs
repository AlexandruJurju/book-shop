using BookShop.Users.Application.Abstractions.Idempotency;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Domain;

namespace BookShop.Users.Application;

public abstract class IdempotentDomainEventHandler<TNotification>(
    IDomainEventConsumerRepository consumerRepository
) : IDomainEventHandler<TNotification>
    where TNotification : IDomainEvent
{
    public async Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        string handlerName = GetType().Name;

        if (await consumerRepository.ExistsAsync(notification.Id, handlerName, cancellationToken))
        {
            return;
        }

        await HandleAsync(notification, cancellationToken);

        await consumerRepository.AddAsync(notification.Id, handlerName, cancellationToken);
    }

    protected abstract Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
}
