using Ardalis.Result;

namespace BookShop.Catalog.Domain.Books.ValueObjects;

public sealed record BookTitle
{
    public const int MaxLength = 300;

    public string Value { get; }

    private BookTitle(string value) => Value = value;

    public static Result<BookTitle> From(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result.Invalid(new ValidationError(nameof(title), "Title cannot be empty."));
        }

        title = title.Trim();

        if (title.Length > MaxLength)
        {
            return Result.Invalid(new ValidationError(nameof(title), $"Title cannot exceed {MaxLength} characters."));
        }

        return new BookTitle(title);
    }
}
