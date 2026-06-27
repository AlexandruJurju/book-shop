using System.Reflection;
using BookShop.Cart.Presentation;

namespace BookShop.Cart.ArchitectureTests.Abstractions;

internal abstract class BaseTest
{
    protected static readonly Assembly Presentation = typeof(AssemblyMarker).Assembly;
    protected static readonly Assembly Application = typeof(AssemblyMarker).Assembly;
    protected static readonly Assembly Domain = typeof(AssemblyMarker).Assembly;
    protected static readonly Assembly Infrastructure = typeof(AssemblyMarker).Assembly;
}
