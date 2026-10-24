using Ardalis.Result;
using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Genres;

public sealed class Genre : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public Guid? ParentGenreId { get; private set; }

    private Genre() { } // EF Core

    private Genre(Guid id, string name, Guid? parentGenreId)
        : base(id)
    {
        Name = name;
        ParentGenreId = parentGenreId;
    }

    // todo: check genre cannot be own parent
    public static Result<Genre> Create(string name, Guid? parentGenreId = null)
    {
        return new Genre(Guid.CreateVersion7(), name, parentGenreId);
    }
}
