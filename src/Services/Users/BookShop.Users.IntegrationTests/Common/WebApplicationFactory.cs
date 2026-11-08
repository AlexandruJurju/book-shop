using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TUnit.AspNetCore;

namespace BookShop.Users.IntegrationTests.Common;

internal sealed class WebApplicationFactory : TestWebApplicationFactory<Program>
{
    [ClassDataSource<Postgres>(Shared = SharedType.PerTestSession)]
    public Postgres Postgres { get; init; } = null!;

    [ClassDataSource<Keycloak>(Shared = SharedType.PerTestSession)]
    public Keycloak Keycloak { get; init; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:postgres", Postgres.Container.GetConnectionString());
        builder.UseSetting("ConnectionStrings:keycloak", Keycloak.Container.GetConnectionString());

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:postgres", Postgres.Container.GetConnectionString() }
            });
        });
    }
}
