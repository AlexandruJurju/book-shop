namespace BuildingBlocks.Domain;

public abstract class Entity<TId> : HasDomainEventsBase
    where TId : notnull
{
    public TId Id { get; protected init; } = default!;

    // for EF Core
    protected Entity() { }

    protected Entity(TId id)
    {
        Id = id;
    }
}
