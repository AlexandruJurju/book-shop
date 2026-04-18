namespace BookShop.Users.Application.Caching;

public static class CacheKeys
{
    public static string User(Guid userId)
    {
        return $"user:{userId}";
    }
}
