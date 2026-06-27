using System.Reflection;

namespace BookShop.Users.Presentation;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}
