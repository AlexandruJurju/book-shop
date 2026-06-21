using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using TUnit.Core.Interfaces;

namespace BookShop.Cart.IntegrationTests.Common;

internal sealed class DockerNetwork : IAsyncInitializer, IAsyncDisposable
{
    public INetwork Instance { get; } = new NetworkBuilder()
        .WithName($"tunit-{Guid.NewGuid():N}")
        .Build();

    public async Task InitializeAsync() => await Instance.CreateAsync();
    public async ValueTask DisposeAsync() => await Instance.DisposeAsync();
}
