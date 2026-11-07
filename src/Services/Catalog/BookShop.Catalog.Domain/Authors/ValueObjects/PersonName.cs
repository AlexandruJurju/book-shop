using Ardalis.Result;

namespace BookShop.Catalog.Domain.Authors.ValueObjects;

public sealed record PersonName
{
    public const int MaxLength = 200;

    public string Value { get; }

    private PersonName(string value) => Value = value;

    public static Result<PersonName> From(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Invalid(new ValidationError
            {
                Identifier = nameof(name),
                ErrorMessage = "Name cannot be empty."
            });
        }

        name = name.Trim().Normalize();

        if (name.Length > MaxLength)
        {
            return Result.Invalid(new ValidationError
            {
                Identifier = nameof(name),
                ErrorMessage = $"Name cannot exceed {MaxLength} characters."
            });
        }

        return new PersonName(name);
    }
}
