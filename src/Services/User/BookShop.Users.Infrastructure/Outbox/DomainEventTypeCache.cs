using System.Collections.Concurrent;
using BookShop.Users.Domain.Users;

namespace BookShop.Users.Infrastructure.Outbox;

internal static class DomainEventTypeCache
{
    private static readonly ConcurrentDictionary<string, Type> TypeCache = new();

    public static Type GetOrAdd(string typeName)
    {
        return TypeCache.GetOrAdd(typeName, static name => typeof(User).Assembly.GetType(name)!);
    }
}
