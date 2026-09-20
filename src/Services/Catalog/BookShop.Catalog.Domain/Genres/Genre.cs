using Ardalis.Result;
using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Genres;

public sealed class Genre : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid? ParentGenreId { get; private set; }

    private Genre() { } // EF Core

    private Genre(Guid id, string name, string? description, Guid? parentGenreId)
        : base(id)
    {
        Name = name;
        Description = description;
        ParentGenreId = parentGenreId;
    }

    public static Result<Genre> Create(string name, string? description = null, Guid? parentGenreId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Error(GenreErrors.NameRequired);
        }

        return new Genre(Guid.CreateVersion7(), name.Trim(), description, parentGenreId);
    }

    public Result Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Error(GenreErrors.NameRequired);
        }

        Name = name.Trim();

        return Result.Success();
    }

    public Result UpdateDescription(string? description)
    {
        Description = description;
        return Result.Success();
    }

    public Result SetParentGenre(Guid? parentGenreId)
    {
        if (parentGenreId == Id)
        {
            return Result.Error(GenreErrors.CannotBeOwnParent);
        }

        ParentGenreId = parentGenreId;

        return Result.Success();
    }
}
