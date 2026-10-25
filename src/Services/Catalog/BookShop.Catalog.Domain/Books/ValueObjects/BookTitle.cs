using Ardalis.Result;

namespace BookShop.Catalog.Domain.Books.ValueObjects;

public sealed record BookTitle
{
    public string Value { get; }

    private BookTitle(string value)
    {
        Value = value;
    }

    private const int MaxLength = 300;

    public static Result<BookTitle> From(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result.Error("Title cannot be empty.");
        }

        title = title.Trim();

        if (title.Length > MaxLength)
        {
            return Result.Error("Title cannot exceed 300 characters.");
        }

        return new BookTitle(title);
    }
}
