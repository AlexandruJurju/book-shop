using System.Reflection;

namespace BookShop.Catalog;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
