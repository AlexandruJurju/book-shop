using Ardalis.Result;
using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Authors;

public sealed class Author : Entity, IAggregateRoot
{
    public PersonName Name { get; private set; }
    public string? Biography { get; private set; }
    public DateOnly? BirthDate { get; private set; }
    public DateOnly? DeathDate { get; private set; }

    private Author() { } // EF Core

    private Author(Guid id, PersonName name, string? biography, DateOnly? birthDate, DateOnly? deathDate)
        : base(id)
    {
        Name = name;
        Biography = biography;
        BirthDate = birthDate;
        DeathDate = deathDate;
    }

    public static Result<Author> Create(PersonName name, string? biography = null, DateOnly? birthDate = null, DateOnly? deathDate = null)
    {
        if (deathDate < birthDate)
        {
            return Result.Error(AuthorErrors.InvalidDates);
        }

        return new Author(Guid.CreateVersion7(), name, biography, birthDate, deathDate);
    }

    public Result UpdateBiography(string? biography)
    {
        Biography = biography;
        return Result.Success();
    }
}
