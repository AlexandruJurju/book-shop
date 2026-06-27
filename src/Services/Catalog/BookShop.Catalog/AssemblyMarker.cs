using System.Reflection;

namespace BookShop.Catalog;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}
