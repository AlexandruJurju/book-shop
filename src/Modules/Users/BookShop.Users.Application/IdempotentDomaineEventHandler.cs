using BookShop.Users.Application.Abstractions.Idempotency;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Domain;

namespace BookShop.Users.Application;

public abstract class IdempotentDomainEventHandler<TNotification>(
    IIdempotencyDomainEventRepository idempotencyRepository
) : IDomainEventHandler<TNotification>
    where TNotification : IDomainEvent
{
    public async ValueTask Handle(TNotification notification, CancellationToken cancellationToken)
    {
        string handlerName = GetType().Name;

        if (await idempotencyRepository.ExistsAsync(notification.Id, handlerName, cancellationToken))
        {
            return;
        }

        await HandleAsync(notification, cancellationToken);

        await idempotencyRepository.AddAsync(notification.Id, handlerName, cancellationToken);
    }

    protected abstract ValueTask HandleAsync(TNotification notification, CancellationToken cancellationToken);
}
