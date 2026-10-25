using Ardalis.Result;

namespace BookShop.Catalog.Domain.Books.ValueObjects;

public sealed record Isbn
{
    public string Value { get; }

    private Isbn(string value)
    {
        Value = value;
    }

    public static Result<Isbn> From(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return Result.Error("ISBN cannot be empty.");
        }

        string normalized = raw.Replace("-", "").Replace(" ", "").ToUpperInvariant();

        if (normalized.Length is not (10 or 13))
        {
            return Result.Error("ISBN must be 10 or 13 characters.");
        }

        if (!IsValidChecksum(normalized))
        {
            return Result.Error($"'{raw}' is not a valid ISBN.");
        }

        return Result.Success(new Isbn(normalized));
    }

    private static bool IsValidChecksum(string isbn) => isbn.Length == 10 ? IsValidIsbn10(isbn) : IsValidIsbn13(isbn);

    private static bool IsValidIsbn10(string isbn)
    {
        int sum = 0;

        for (int i = 0; i < 10; i++)
        {
            char character = isbn[i];

            int digit =
                character == 'X' && i == 9 ? 10
                : char.IsDigit(character) ? character - '0'
                : -1;

            if (digit < 0)
            {
                return false;
            }

            sum += digit * (10 - i);
        }

        return sum % 11 == 0;
    }

    private static bool IsValidIsbn13(string isbn)
    {
        if (!isbn.All(char.IsDigit))
        {
            return false;
        }

        int sum = 0;

        for (int i = 0; i < 13; i++)
        {
            int digit = isbn[i] - '0';

            sum += i % 2 == 0 ? digit : digit * 3;
        }

        return sum % 10 == 0;
    }
}
