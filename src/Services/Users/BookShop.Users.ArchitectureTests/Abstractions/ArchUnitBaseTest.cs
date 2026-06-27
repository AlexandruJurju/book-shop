using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;

namespace BookShop.Users.ArchitectureTests.Abstractions;

internal abstract class ArchUnitBaseTest : BaseTest
{
    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            DomainAssembly,
            ApplicationAssembly,
            InfrastructureAssembly,
            PresentationAssembly
        )
        .Build();
}
