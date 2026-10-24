using Ardalis.Result;
using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Authors;

public sealed class Author : Entity, IAggregateRoot
{
    public PersonName Name { get; private set; }

    private Author() { } // EF Core

    private Author(Guid id, PersonName name)
        : base(id)
    {
        Name = name;
    }

    public static Result<Author> Create(PersonName name)
    {
        return new Author(Guid.CreateVersion7(), name);
    }
}
