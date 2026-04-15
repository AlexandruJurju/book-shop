using System;
using System.Reflection;

namespace BookShop.Catalog.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    public static readonly Type MarkerType = typeof(AssemblyReference);
}
