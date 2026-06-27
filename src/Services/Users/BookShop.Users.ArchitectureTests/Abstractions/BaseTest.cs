using System.Reflection;
using BookShop.Users.Domain.Users;
using BookShop.Users.Infrastructure;

namespace BookShop.Users.ArchitectureTests.Abstractions;

internal abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Application.AssemblyMarker).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(User).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(DependencyInjection).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Presentation.AssemblyMarker).Assembly;
}
