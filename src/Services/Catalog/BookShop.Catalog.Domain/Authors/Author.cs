using Ardalis.Result;
using BookShop.Catalog.Domain.Authors.ValueObjects;
using BookShop.Catalog.Domain.Books;
using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Authors;

public sealed class Author : Entity<Guid>, IAggregateRoot
{
    public PersonName Name { get; private set; }

    public IReadOnlyCollection<BookAuthor> BookAuthors => _bookAuthors.AsReadOnly();
    private readonly List<BookAuthor> _bookAuthors = [];

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
