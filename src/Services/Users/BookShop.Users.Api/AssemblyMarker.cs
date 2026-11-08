using System.Reflection;

namespace BookShop.Users.Api;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}
