using BuildingBlocks.Domain;

namespace BuildingBlocks.Application.CQRS;

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T domainEvent, CancellationToken cancellationToken = default);
}
