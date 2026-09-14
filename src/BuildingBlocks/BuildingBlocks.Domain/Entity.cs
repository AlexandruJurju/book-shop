namespace BuildingBlocks.Domain;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    private readonly List<DomainEvent> _domainEvents = [];
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}

public abstract class Entity<TId> : Entity
    where TId : IEquatable<TId>
{
    public new TId Id { get; init; }
}
