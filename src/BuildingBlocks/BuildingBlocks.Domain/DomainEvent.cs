namespace BuildingBlocks.Domain;

public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; init; }

    public DateTime OccurredAtUtc { get; init; }
}
