using System.Net.Mail;
using Ardalis.Result;

namespace BookShop.Users.Domain.Users;

public sealed record Email
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Invalid(new ValidationError("Email cannot be empty."));
        }

        value = value.Trim();

        try
        {
            var address = new MailAddress(value);

            if (address.Address != value)
            {
                return Result.Invalid(new ValidationError("Invalid email address."));
            }
        }
        catch (FormatException)
        {
            return Result.Invalid(new ValidationError("Invalid email address."));
        }

        return Result.Success(new Email(value));
    }

    public override string ToString() => Value;
}
