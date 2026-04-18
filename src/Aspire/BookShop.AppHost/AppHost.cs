using BookShop.Shared.Aspire;
using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> pgUser = builder.AddParameter("postgres-user", "app_user", secret: false);
IResourceBuilder<ParameterResource> pgPass =
    builder.AddParameter("postgres-password", "super-secret-password", secret: true);
IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres(Resources.Postgres, port: 5432)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithUserName(pgUser)
    .WithPassword(pgPass);

IResourceBuilder<PostgresDatabaseResource> catalogDb = postgres.AddDatabase(CatalogResources.Database);
IResourceBuilder<PostgresDatabaseResource> usersDb = postgres.AddDatabase(UsersResources.Database);

builder.AddProject<BookShop_WebApi>("bookshop-webapi")
    .WithReference(catalogDb).WaitFor(catalogDb)
    .WithReference(usersDb).WaitFor(usersDb)
    ;

await builder.Build().RunAsync();
