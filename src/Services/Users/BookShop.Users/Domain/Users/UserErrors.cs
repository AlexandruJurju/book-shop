namespace BookShop.Users.Domain.Users;

public static class UserErrors
{
    public static string UserExists(string email)
    {
        return $"There already is an user with the email {email}";
    }
    
    public static string NotFound(Guid id)
    {
        return $"User with id {id} not found";
    }
}
