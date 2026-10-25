using Ardalis.Result;

namespace BookShop.Catalog.Domain.Authors.ValueObjects;

public sealed record PersonName
{
    public string Value { get; }

    private PersonName(string value)
    {
        Value = value;
    }

    public static Result<PersonName> From(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Error("Name cannot be empty.");
        }

        if (name.Length > 200)
        {
            return Result.Error("Name is too long.");
        }

        return Result.Success(new PersonName(name));
    }
}
