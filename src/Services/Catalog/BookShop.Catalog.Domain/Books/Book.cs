using Ardalis.Result;
using BookShop.Catalog.Domain.Authors;
using BookShop.Catalog.Domain.Books.Events;
using BookShop.Catalog.Domain.Genres;
using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Books;

public sealed class Book : Entity, IAggregateRoot
{
    public BookTitle Title { get; private set; }
    public string? Description { get; private set; }
    public DateOnly OriginalPublicationDate { get; private set; }
    public BookStatus Status { get; private set; }

    private readonly List<BookAuthor> _authors = [];
    private readonly List<BookEdition> _editions = [];
    private readonly List<Genre> _genres = [];
    public IReadOnlyCollection<BookAuthor> Authors => _authors.AsReadOnly();
    public IReadOnlyCollection<Genre> Genres => _genres.AsReadOnly();
    public IReadOnlyCollection<BookEdition> Editions => _editions.AsReadOnly();

    private Book() { } // EF Core

    private Book(Guid id, BookTitle title, IEnumerable<Genre> genres, DateOnly originalPublicationDate, string? description)
        : base(id)
    {
        Title = title;
        Description = description;
        OriginalPublicationDate = originalPublicationDate;
        Status = BookStatus.Draft;

        _genres.AddRange(genres.DistinctBy(x => x.Id));
    }

    public static Result<Book> Create(
        BookTitle title,
        IEnumerable<Author> authors,
        IEnumerable<Genre> genres,
        DateOnly originalPublicationDate,
        string? description = null
    )
    {
        Author[] distinctAuthors = authors.DistinctBy(x => x.Id).ToArray();

        if (distinctAuthors.Length == 0)
        {
            return Result.Error(BookErrors.NoAuthor);
        }

        Genre[] distinctGenres = genres.DistinctBy(x => x.Id).ToArray();

        var book = new Book(Guid.CreateVersion7(), title, distinctGenres, originalPublicationDate, description);

        foreach (Author author in distinctAuthors)
        {
            book.AddAuthor(author.Id);
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

    public Result RemoveAuthor(Guid authorId)
    {
        BookAuthor? author = _authors.SingleOrDefault(a => a.AuthorId == authorId);

        if (author is null)
        {
            return Result.NotFound("Author association was not found.");
        }

        _authors.Remove(author);

        for (int i = 0; i < _authors.Count; i++)
        {
            _authors[i].SetOrder(i);
        }

        return Result.Success();
    }

    public Result AddGenre(Genre genre)
    {
        if (_genres.Any(g => g.Id == genre.Id))
        {
            return Result.Conflict("Genre already assigned.");
        }

        _genres.Add(genre);
        return Result.Success();
    }
}
