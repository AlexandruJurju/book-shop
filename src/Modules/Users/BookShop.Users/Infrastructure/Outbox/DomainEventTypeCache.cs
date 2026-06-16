using System.Collections.Concurrent;

namespace BookShop.Users.Infrastructure.Outbox;

internal sealed class DomainEventTypeCache
{
    private static readonly ConcurrentDictionary<string, Type> TypeCache = new();

    public static Type GetOrAdd(string typeName)
    {
        return TypeCache.GetOrAdd(typeName, static name => AssemblyMarker.Assembly.GetType(name)!);
    }
}
