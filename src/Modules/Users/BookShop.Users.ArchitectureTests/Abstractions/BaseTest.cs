using System.Reflection;
using BookShop.Users;

namespace BookShop.Users.ArchitectureTests.Abstractions;

internal abstract class BaseTest
{
    protected static readonly Assembly UsersAssembly = typeof(AssemblyMarker).Assembly;
}
