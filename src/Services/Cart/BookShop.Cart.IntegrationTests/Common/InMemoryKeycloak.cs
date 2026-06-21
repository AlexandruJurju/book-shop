using System.Diagnostics.CodeAnalysis;
using Testcontainers.Keycloak;
using TUnit.Core.Interfaces;

namespace BookShop.Cart.IntegrationTests.Common;

internal sealed class InMemoryKeycloak : IAsyncInitializer, IAsyncDisposable
{
    [ClassDataSource<DockerNetwork>(Shared = SharedType.PerTestSession)]
    public required DockerNetwork DockerNetwork { get; init; }

    [field: AllowNull, MaybeNull]
    public KeycloakContainer Container => field ??= new KeycloakBuilder("quay.io/keycloak/keycloak:26.5")
        .WithNetwork(DockerNetwork.Instance)
        .Build();

    public async Task InitializeAsync() => await Container.StartAsync();

    public async ValueTask DisposeAsync() => await Container.DisposeAsync();
}
