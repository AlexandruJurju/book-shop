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
    // public void Domain_Layer_Has_No_Dependencies_On_Other_Layers()
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
    public void Application_Layer_Depends_Only_On_Domain()
    {
        IArchRule rule = Types().That().ResideInNamespaceMatching(ApplicationNamespace)
            .Should().NotDependOnAny(
                Types()
                    .That().ResideInNamespaceMatching(InfrastructureNamespace)
                    .Or().ResideInNamespaceMatching(PresentationNamespace)
            )
            .Because("Application layer must depend only on domain");

        rule.Check(Architecture);
    }

    [Test]
    public void Infrastructure_Layer_Does_Not_Depend_On_Presentation()
    {
        IArchRule rule = Types().That().ResideInNamespaceMatching(InfrastructureNamespace)
            .Should().NotDependOnAny(
                Types()
                    .That().ResideInNamespaceMatching(PresentationNamespace)
            )
            .Because("Infrastructure must not depend on Presentation");

        rule.Check(Architecture);
    }
}
