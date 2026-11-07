using BookShop.Catalog.Domain.Authors;

namespace BookShop.Catalog.Domain.Books;

public sealed class BookAuthor
{
    public Guid BookId { get; private set; }
    public Book Book { get; private set; } = null!;

    public Guid AuthorId { get; private set; }
    public Author Author { get; private set; } = null!;

    public int Order { get; private set; }

    private BookAuthor() { } // EF Core

    internal BookAuthor(Guid authorId, int order)
    {
        AuthorId = authorId;
        Order = order;
    }

    internal void SetOrder(int order)
    {
        Order = order;
    }
}
