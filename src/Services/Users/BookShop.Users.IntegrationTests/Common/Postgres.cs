using Testcontainers.PostgreSql;
using TUnit.Core.Interfaces;

namespace BookShop.Users.IntegrationTests.Common;

internal sealed class Postgres : IAsyncInitializer, IAsyncDisposable
{
    public PostgreSqlContainer Container { get; } = new PostgreSqlBuilder("docker.io/library/postgres:18.3")
        .Build();

    public async Task InitializeAsync() => await Container.StartAsync();
    public async ValueTask DisposeAsync() => await Container.DisposeAsync();
}
