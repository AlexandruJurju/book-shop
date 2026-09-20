using Ardalis.Result;

namespace BookShop.Catalog.Domain.Books;

public sealed record Language
{
    public string Code { get; }

    private Language(string code)
    {
        Code = code;
    }

    public static Result<Language> Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 2)
        {
            return Result.Invalid(new ValidationError("Language must be a 2-letter ISO 639-1 code.", nameof(code)));
        }

        return new Language(code.ToLowerInvariant());
    }
}
