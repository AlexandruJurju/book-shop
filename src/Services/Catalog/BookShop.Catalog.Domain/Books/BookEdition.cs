using Ardalis.Result;
using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Books;

public sealed class BookEdition : Entity
{
    public Isbn Isbn { get; private set; }
    public Guid PublisherId { get; private set; }
    public BookFormat Format { get; private set; }
    public Language Language { get; private set; }
    public DateOnly PublicationDate { get; private set; }
    public int PageCount { get; private set; }

    private BookEdition() { } // EF Core

    private BookEdition(
        Guid id,
        Isbn isbn,
        Guid publisherId,
        BookFormat format,
        Language language,
        DateOnly publicationDate,
        int pageCount
    )
        : base(id)
    {
        Isbn = isbn;
        PublisherId = publisherId;
        Format = format;
        Language = language;
        PublicationDate = publicationDate;
        PageCount = pageCount;
    }

    public static Result<BookEdition> Create(
        Isbn isbn,
        Guid publisherId,
        BookFormat format,
        Language language,
        DateOnly publicationDate,
        int pageCount
    )
    {
        if (pageCount < 0)
        {
            return Result.Error(BookEditionErrors.NegativePageCount);
        }

        return new BookEdition(
            Guid.CreateVersion7(),
            isbn,
            publisherId,
            format,
            language,
            publicationDate,
            pageCount);
    }
}
