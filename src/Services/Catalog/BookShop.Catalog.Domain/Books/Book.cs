using Ardalis.Result;
using BookShop.Catalog.Domain.Books.Events;
using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Books;

public sealed class Book : Entity, IAggregateRoot
{
    private readonly List<Guid> _authorIds = [];
    private readonly List<BookEdition> _editions = [];
    private readonly List<Guid> _genreIds = [];

    public BookTitle Title { get; private set; }
    public string? Description { get; private set; }
    public DateOnly OriginalPublicationDate { get; private set; }
    public BookStatus Status { get; private set; }

    public IReadOnlyCollection<Guid> AuthorIds => _authorIds.AsReadOnly();
    public IReadOnlyCollection<Guid> GenreIds => _genreIds.AsReadOnly();
    public IReadOnlyCollection<BookEdition> Editions => _editions.AsReadOnly();

    private Book() { } // EF Core

    private Book(
        Guid id,
        BookTitle title,
        IEnumerable<Guid> authorIds,
        IEnumerable<Guid> genres,
        DateOnly originalPublicationDate,
        string? description
    )
        : base(id)
    {
        Title = title;
        Description = description;
        OriginalPublicationDate = originalPublicationDate;
        Status = BookStatus.Draft;

        _authorIds.AddRange(authorIds.Distinct());
        _genreIds.AddRange(genres.Distinct());
    }

    public static Result<Book> Create(
        BookTitle title,
        IEnumerable<Guid> authorIds,
        IEnumerable<Guid> genres,
        DateOnly originalPublicationDate,
        string? description = null
    )
    {
        Guid[] distinctAuthorIds = authorIds.Distinct().ToArray();
        Guid[] distinctGenres = genres.Distinct().ToArray();

        if (distinctAuthorIds.Length == 0)
        {
            return Result.Error(BookErrors.NoAuthor);
        }

        var book = new Book(Guid.CreateVersion7(), title, distinctAuthorIds, distinctGenres, originalPublicationDate, description);

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
        if (_authorIds.Contains(authorId))
        {
            return Result.Error(BookErrors.AuthorAlreadyExists);
        }

        _authorIds.Add(authorId);

        return Result.Success();
    }

    public Result RemoveAuthor(Guid authorId)
    {
        if (!_authorIds.Remove(authorId))
        {
            return Result.Error(BookErrors.AuthorNotFound);
        }

        if (_authorIds.Count == 0)
        {
            return Result.Error(BookErrors.NoAuthor);
        }

        return Result.Success();
    }

    public Result AddGenre(Guid genreId)
    {
        if (_genreIds.Contains(genreId))
        {
            return Result.Error(BookErrors.GenreAlreadyExists);
        }

        _genreIds.Add(genreId);

        return Result.Success();
    }

    public Result RemoveGenre(Guid genreId)
    {
        return !_genreIds.Remove(genreId) ? Result.Error(BookErrors.GenreNotFound) : Result.Success();
    }
}
