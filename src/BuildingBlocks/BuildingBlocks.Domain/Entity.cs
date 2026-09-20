namespace BuildingBlocks.Domain;

public abstract class Entity
{
    public Guid Id { get; init; }

    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity() { }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
