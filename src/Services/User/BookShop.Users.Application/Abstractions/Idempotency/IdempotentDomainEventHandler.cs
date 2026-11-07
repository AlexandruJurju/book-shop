using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Domain;
using Microsoft.Extensions.Logging;

namespace BookShop.Users.Application.Abstractions.Idempotency;

public abstract class IdempotentDomainEventHandler<TDomainEvent>(
    IDomainEventConsumerRepository consumerRepository,
    ILogger logger
) : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public async ValueTask Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        string consumerName = GetType().Name;

        if (await consumerRepository.ExistsAsync(notification.Id, consumerName, cancellationToken))
        {
            logger.LogInformation(
                "Skipping domain event handler {ConsumerName} for domain event {DomainEventId}",
                consumerName,
                notification.Id);
            return;
        }

        await HandleIdempotent(notification, cancellationToken);

        await consumerRepository.AddAsync(notification.Id, consumerName, cancellationToken);
    }

    protected abstract ValueTask HandleIdempotent(
        TDomainEvent notification,
        CancellationToken cancellationToken
    );
}
