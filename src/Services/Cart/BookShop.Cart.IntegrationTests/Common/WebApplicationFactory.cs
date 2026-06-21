using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TUnit.AspNetCore;

namespace BookShop.Cart.IntegrationTests.Common;

internal sealed class WebApplicationFactory : TestWebApplicationFactory<Program>
{
    [ClassDataSource<InMemoryPostgres>(Shared = SharedType.PerTestSession)]
    public InMemoryPostgres Postgres { get; init; } = null!;

    [ClassDataSource<InMemoryKeycloak>(Shared = SharedType.PerTestSession)]
    public InMemoryKeycloak Keycloak { get; init; } = null!;

    protected override void ConfigureStartupConfiguration(IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "ConnectionStrings:postgres", Postgres.Container.GetConnectionString() },
            { "Keycloak:BaseUrl", Keycloak.Container.GetConnectionString() },
        });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Postgres:ConnectionString", Postgres.Container.GetConnectionString() },
                { "Keycloak:ConnectionString", Keycloak.Container.GetConnectionString() },
            });
        });
    }
}
