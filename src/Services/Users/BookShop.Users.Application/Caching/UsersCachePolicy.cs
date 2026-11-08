namespace BookShop.Users.Application.Caching;

public static class UsersCachePolicy
{
    public static string User(Guid userId) => $"user:{userId}";
    public static string Identity(string identityId) => $"identity:{identityId}";
}
