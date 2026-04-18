using BuildingBlocks.Common;

namespace BookShop.Users.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(string email)
    {
        return Error.Conflict(
            "Users.Exists",
            $"There already is an user with the email {email}"
        );
    }
}
