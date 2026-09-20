using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Books.Events;

public sealed class BookEditionAddedEvent : DomainEvent
{
    public Guid BookId { get; init; }
    public Guid EditionId { get; init; }
    public BookFormat Format { get; init; }

    public BookEditionAddedEvent(Guid bookId, Guid editionId, BookFormat format)
    {
        BookId = bookId;
        EditionId = editionId;
        Format = format;
    }
}
