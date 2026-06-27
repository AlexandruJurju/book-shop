using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;

namespace BookShop.Cart.ArchitectureTests.Abstractions;

internal abstract class ArchUnitBaseTest : BaseTest
{
    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(
            Presentation
        )
        .Build();

    protected static readonly IObjectProvider<IType> UserServiceTypes = ArchRuleDefinition
        .Types()
        .That()
        .ResideInAssembly(Presentation)
        .As(nameof(Presentation));
}
