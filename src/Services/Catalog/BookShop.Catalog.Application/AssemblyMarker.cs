using System.Reflection;

namespace BookShop.Catalog.Application;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}
