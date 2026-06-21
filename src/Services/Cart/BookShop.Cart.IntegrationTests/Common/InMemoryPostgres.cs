using System.Diagnostics.CodeAnalysis;
using Testcontainers.PostgreSql;
using TUnit.Core.Interfaces;

namespace BookShop.Cart.IntegrationTests.Common;

internal sealed class InMemoryPostgres : IAsyncInitializer, IAsyncDisposable
{
    [ClassDataSource<DockerNetwork>(Shared = SharedType.PerTestSession)]
    public required DockerNetwork DockerNetwork { get; init; }

    [field: AllowNull, MaybeNull]
    public PostgreSqlContainer Container => field ??= new PostgreSqlBuilder("docker.io/library/postgres:18.3")
        .WithDatabase("TestDatabase")
        .WithNetwork(DockerNetwork.Instance)
        .Build();

    public async Task InitializeAsync() => await Container.StartAsync();

    public async ValueTask DisposeAsync() => await Container.DisposeAsync();
}
