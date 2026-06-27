using BuildingBlocks.Domain;

namespace BuildingBlocks.Application.CQRS;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
