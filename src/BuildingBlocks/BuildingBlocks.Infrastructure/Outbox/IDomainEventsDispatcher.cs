using BuildingBlocks.Domain;

namespace BuildingBlocks.Infrastructure.Outbox;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default);
}
