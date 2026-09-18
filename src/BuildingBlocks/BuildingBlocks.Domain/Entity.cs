namespace BuildingBlocks.Domain;

public abstract class Entity
{
    public Guid Id { get; protected init; }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}

public abstract class Entity<TId> : Entity
    where TId : IEquatable<TId>
{
    public new TId Id { get; init; }
}
