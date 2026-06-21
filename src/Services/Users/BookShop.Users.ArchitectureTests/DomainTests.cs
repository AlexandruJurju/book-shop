using ArchUnitNET.TUnit;
using BookShop.Users.ArchitectureTests.Abstractions;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace BookShop.Users.ArchitectureTests;

internal sealed class DomainTests : ArchUnitBaseTest
{
    private const string DomainNamespace = $"{nameof(BookShop)}.{nameof(Users)}.{nameof(Domain)}";

    [Test]
    public void Domain_Classes_Are_Sealed()
    {
        Classes()
            .That()
            .ResideInNamespaceMatching(DomainNamespace)
            .Should()
            .BeSealed()
            .Because(
                "Classes in the domain layer should be sealed"
            )
            .Check(Architecture);
    }

    [Test]
    public void Domain_Classes_Are_Public()
    {
        Classes()
            .That()
            .ResideInNamespaceMatching(DomainNamespace)
            .Should()
            .BePublic()
            .Because(
                "Classes in the domain layer should be public"
            )
            .Check(Architecture);
    }
}
