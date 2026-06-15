using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;

namespace BookShop.Users.ArchitectureTests.Abstractions;

internal abstract class ArchUnitBaseTest : BaseTest
{
    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(
            UsersAssembly
        )
        .Build();

    protected static readonly IObjectProvider<IType> UserServiceTypes = ArchRuleDefinition
        .Types()
        .That()
        .ResideInAssembly(UsersAssembly)
        .As(nameof(UsersAssembly));
}
