using BuildingBlocks.Domain;

namespace BuildingBlocks.Application.CQRS;

public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public abstract Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default);

    public Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken = default) =>
        HandleAsync((TDomainEvent)domainEvent, cancellationToken);
}
