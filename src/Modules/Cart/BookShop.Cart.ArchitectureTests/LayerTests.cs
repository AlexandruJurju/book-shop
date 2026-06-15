using ArchUnitNET.Fluent;
using ArchUnitNET.TUnit;
using BookShop.Cart.ArchitectureTests.Abstractions;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace BookShop.Cart.ArchitectureTests;

internal sealed class LayerTests : ArchUnitBaseTest
{
    // private const string DomainNamespace = $"{nameof(BookShop)}.{nameof(Users)}.{nameof(Domain)}.*";
    private const string ApplicationNamespace = $"{nameof(BookShop)}.{nameof(Cart)}.{nameof(Application)}.*";
    private const string PresentationNamespace = $"{nameof(BookShop)}.{nameof(Cart)}.{nameof(Presentation)}.*";
    private const string InfrastructureNamespace = $"{nameof(BookShop)}.{nameof(Cart)}.{nameof(Infrastructure)}.*";

    // [Test]
    // public void DomainLayer_ShouldRespectCleanArchitectureDependencies()
    // {
    //     IArchRule rule = Types().That().ResideInNamespaceMatching(DomainNamespace)
    //         .Should().NotDependOnAny(
    //             Types()
    //                 .That().ResideInNamespaceMatching(InfrastructureNamespace)
    //                 .Or().ResideInNamespaceMatching(PresentationNamespace)
    //                 .Or().ResideInNamespaceMatching(ApplicationNamespace)
    //         )
    //         .Because("The Domain layer must not depend on outer layers");
    //
    //     rule.Check(Architecture);
    // }

    [Test]
    public void ApplicationLayer_ShouldRespectCleanArchitectureDependencies()
    {
        IArchRule rule = Types().That().ResideInNamespaceMatching(ApplicationNamespace)
            .Should().NotDependOnAny(
                Types()
                    .That().ResideInNamespaceMatching(InfrastructureNamespace)
                    .Or().ResideInNamespaceMatching(PresentationNamespace)
            )
            .Because("The Domain layer must not depend on outer layers");

        rule.Check(Architecture);
    }
    
    [Test]
    public void InfrastructureLayer_ShouldRespectCleanArchitectureDependencies()
    {
        IArchRule rule = Types().That().ResideInNamespaceMatching(InfrastructureNamespace)
            .Should().NotDependOnAny(
                Types()
                    .That().ResideInNamespaceMatching(PresentationNamespace)
            )
            .Because("The Domain layer must not depend on outer layers");

        rule.Check(Architecture);
    }
}
