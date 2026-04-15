using Projects;
using Shared.Constants.Aspire;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> pgUser = builder.AddParameter("postgres-user", "app_user", secret: false);
IResourceBuilder<ParameterResource> pgPass = builder.AddParameter("postgres-password", "super-secret-password", secret: true);
IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres(Resources.Postgres)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithUserName(pgUser)
    .WithPassword(pgPass);

IResourceBuilder<PostgresDatabaseResource> catalogDb = postgres.AddDatabase(CatalogResources.Database);

builder
    .AddProject<BookShop_WebApi>("bookshop")
    .WithReference(catalogDb).WaitFor(catalogDb);

await builder.Build().RunAsync();
