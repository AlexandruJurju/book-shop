using Ardalis.Result;
using BookShop.Catalog.Domain.Authors;
using BookShop.Catalog.Domain.Books.Events;
using BookShop.Catalog.Domain.Genres;
using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Books;

public sealed class Book : Entity, IAggregateRoot
{
    private readonly List<BookAuthor> _authors = [];
    private readonly List<Genre> _genres = [];

    public BookTitle Title { get; private set; } = null!;
    public BookStatus Status { get; private set; }

    public IReadOnlyCollection<BookAuthor> BookAuthors => _authors.AsReadOnly();
    public IReadOnlyCollection<Genre> Genres => _genres.AsReadOnly();

    private Book() { } // EF Core

    private Book(Guid id, BookTitle title)
        : base(id)
    {
        Title = title;
        Status = BookStatus.Draft;
    }

    public static Result<Book> Create(BookTitle title, IEnumerable<Author> authors, IEnumerable<Genre> genres)
    {
        Author[] distinctAuthors = authors.DistinctBy(a => a.Id).ToArray();

        if (distinctAuthors.Length == 0)
        {
            return Result.Error(BookErrors.NoAuthor);
        }

        var book = new Book(Guid.CreateVersion7(), title);

        book._genres.AddRange(genres.DistinctBy(g => g.Id));

        for (int i = 0; i < distinctAuthors.Length; i++)
        {
            book._authors.Add(new BookAuthor(distinctAuthors[i].Id, i));
        }

        book.RaiseDomainEvent(new BookAddedToCatalogEvent(book.Id));

        return book;
    }

    public Result AddAuthor(Guid authorId)
    {
        if (_authors.Any(a => a.AuthorId == authorId))
        {
            return Result.Conflict("Author is already associated with this book.");
        }

        _authors.Add(new BookAuthor(authorId, _authors.Count));

        return Result.Success();
    }
}
