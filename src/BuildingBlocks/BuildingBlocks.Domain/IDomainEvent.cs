using Mediator;

namespace BuildingBlocks.Domain;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccurredOnUtc { get; }
}
