using BookShop.Catalog.Domain.Authors;

namespace BookShop.Catalog.Domain.Books;

public sealed class BookAuthor
{
    public Guid BookId { get; private set; }
    public Book Book { get; private set; }

    public Guid AuthorId { get; private set; }
    public Author Author { get; private set; }

    public int Order { get; private set; }

    private BookAuthor() { } // EF Core

    private BookAuthor(Guid bookId, Guid authorId, int order)
    {
        BookId = bookId;
        AuthorId = authorId;
        Order = order;
    }

    internal static BookAuthor Create(Guid bookId, Guid authorId, int order)
    {
        return new BookAuthor(bookId, authorId, order);
    }

    internal void SetOrder(int order)
    {
        Order = order;
    }
}
