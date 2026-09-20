using Ardalis.Result;

namespace BookShop.Catalog.Domain.Books;

public sealed record BookTitle
{
    public string Value { get; init; }

    private BookTitle(string value)
    {
        Value = value;
    }

    private const int MaxLength = 300;

    public static Result<BookTitle> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result.Invalid(new ValidationError("Title cannot be empty."));
        }

        title = title.Trim();

        if (title.Length > MaxLength)
        {
            return Result.Invalid(new ValidationError("Title cannot exceed 300 characters."));
        }

        return new BookTitle(title);
    }
}
