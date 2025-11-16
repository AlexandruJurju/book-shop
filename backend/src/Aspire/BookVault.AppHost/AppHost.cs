var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

var catalogDb = postgres.AddDatabase("catalogDb");

builder
    .AddProject<Projects.BookVault_Catalog_Api>("bookvault-catalog-api")
    .WithReference(catalogDb)
    .WaitFor(catalogDb);

builder.Build().Run();
