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

    private Book(
        Guid id,
        BookTitle title,
        IEnumerable<Genre> genres,
        DateOnly originalPublicationDate,
        string? description
    ) : base(id)
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

    public Result<BookEdition> AddEdition(Isbn isbn, Guid publisherId, BookFormat format, Language language, DateOnly publicationDate, int pageCount)
    {
        if (_editions.Any(e => e.Isbn == isbn))
        {
            return Result.Error(BookErrors.EditionAlreadyExists(isbn.ToString()));
        }

        Result<BookEdition> editionResult = BookEdition.Create(isbn, publisherId, format, language, publicationDate, pageCount);

        if (editionResult.IsError())
        {
            return editionResult;
        }

        BookEdition edition = editionResult.Value;

        _editions.Add(edition);

        RaiseDomainEvent(new BookEditionAddedEvent(Id, edition.Id, format));

        return edition;
    }

    public Result AddAuthor(Guid authorId)
    {
        if (_authors.Any(x => x.AuthorId == authorId))
        {
            return Result.Error();
        }

        _authors.Add(BookAuthor.Create(Id, authorId, _authors.Count));

        return Result.Success();
    }

    public Result RemoveAuthor(Guid authorId)
    {
        BookAuthor? author = _authors.FirstOrDefault(x => x.AuthorId == authorId);

        if (author is null)
        {
            return Result.Error(BookErrors.AuthorNotFound);
        }

        if (_authors.Count == 1)
        {
            return Result.Error(BookErrors.NoAuthor);
        }

        _authors.Remove(author);

        ReorderAuthors();

        return Result.Success();
    }

    public Result MoveAuthor(Guid authorId, int newOrder)
    {
        if (newOrder < 0 || newOrder >= _authors.Count)
        {
            return Result.Error();
        }

        BookAuthor? author = _authors.FirstOrDefault(x => x.AuthorId == authorId);

        if (author is null)
        {
            return Result.Error(BookErrors.AuthorNotFound);
        }

        _authors.Remove(author);
        _authors.Insert(newOrder, author);

        ReorderAuthors();

        return Result.Success();
    }

    public Result AddGenre(Genre genre)
    {
        if (_genres.Any(x => x.Id == genre.Id))
        {
            return Result.Error(BookErrors.GenreAlreadyExists);
        }

        _genres.Add(genre);

        return Result.Success();
    }

    public Result RemoveGenre(Guid genreId)
    {
        Genre? genre = _genres.FirstOrDefault(x => x.Id == genreId);

        if (genre is null)
        {
            return Result.Error(BookErrors.GenreNotFound);
        }

        _genres.Remove(genre);

        return Result.Success();
    }

    private void ReorderAuthors()
    {
        for (int i = 0; i < _authors.Count; i++)
        {
            _authors[i].SetOrder(i);
        }
    }
}
