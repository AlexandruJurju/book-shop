using Testcontainers.Keycloak;
using TUnit.Core.Interfaces;

namespace BookShop.Users.IntegrationTests.Common;

internal sealed class Keycloak : IAsyncInitializer, IAsyncDisposable
{
    public KeycloakContainer Container { get; } = new KeycloakBuilder("quay.io/keycloak/keycloak:26.5")
        .Build();

    public async Task InitializeAsync() => await Container.StartAsync();
    public async ValueTask DisposeAsync() => await Container.DisposeAsync();
}
