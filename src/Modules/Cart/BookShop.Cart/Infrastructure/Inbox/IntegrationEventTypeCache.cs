using System.Collections.Concurrent;
using System.Reflection;
using BuildingBlocks.Application.EventBus;

namespace BookShop.Cart.Infrastructure.Inbox;

internal static class IntegrationEventTypeCache
{
    private static readonly ConcurrentDictionary<string, Type> TypeCache = new();

    public static Type GetOrAdd(string typeName)
    {
        return TypeCache.GetOrAdd(typeName, static name =>
        {
            Type? type = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t is not null)!; }
                })
                .FirstOrDefault(t => t != null
                    && t.FullName == name
                    && typeof(IIntegrationEvent).IsAssignableFrom(t)
                    && !t.IsGenericTypeDefinition
                );

            return type ?? throw new InvalidOperationException(
                $"Could not resolve integration event type '{name}' " +
                $"Ensure the assembly containing this type is loaded");
        });
    }
}
