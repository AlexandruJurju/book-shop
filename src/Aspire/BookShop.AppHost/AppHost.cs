using BookShop.Shared;
using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> pgUser = builder.AddParameter("postgres-user", "app_user", secret: false);
IResourceBuilder<ParameterResource> pgPassword = builder.AddParameter("postgres-password", "super-secret-password", secret: true);
IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres(Resources.Postgres, port: 5432)
    .WithImageTag("18.3")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithUserName(pgUser)
    .WithPassword(pgPassword)
    .WithEndpointProxySupport(false);

IResourceBuilder<ParameterResource> keycloakAdminUsername = builder.AddParameter("keycloak-user", "admin", secret: false);
IResourceBuilder<ParameterResource> keycloakAdminPassword = builder.AddParameter("keycloak-password", "admin", secret: true);
IResourceBuilder<KeycloakResource> keycloak = builder
    .AddKeycloak(Resources.Keycloak, 8080, keycloakAdminUsername, keycloakAdminPassword)
    .WithImageTag("26.5")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithEndpointProxySupport(false);

builder.AddProject<BookShop_WebApi>("bookshop-webapi")
    .WithReference(postgres).WaitFor(postgres)
    .WithReference(keycloak).WaitFor(keycloak);

await builder.Build().RunAsync();
