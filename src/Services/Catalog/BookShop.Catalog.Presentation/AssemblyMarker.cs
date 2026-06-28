using System.Reflection;

namespace BookShop.Catalog.Presentation;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}
