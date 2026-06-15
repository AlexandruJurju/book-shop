using System.Reflection;

namespace BookShop.Cart.ArchitectureTests.Abstractions;

internal abstract class BaseTest
{
    protected static readonly Assembly UsersAssembly = typeof(AssemblyMarker).Assembly;
}
