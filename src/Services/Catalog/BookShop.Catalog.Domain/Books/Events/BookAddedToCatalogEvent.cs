using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Books.Events;

public sealed class BookAddedToCatalogEvent : DomainEvent
{
    public Guid BookId { get; private set; }

    public BookAddedToCatalogEvent(Guid bookId)
    {
        BookId = bookId;
    }
}
