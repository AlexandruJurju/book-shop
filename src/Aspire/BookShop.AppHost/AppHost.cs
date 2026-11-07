using BookShop.Shared;
using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> pgUser = builder.AddParameter("postgres-user", "app_user", secret: false);
IResourceBuilder<ParameterResource> pgPassword = builder.AddParameter("postgres-password", "super-secret-password", secret: true);
IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres(Resources.Postgres, port: 5432)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithUserName(pgUser)
    .WithPassword(pgPassword);

IResourceBuilder<ParameterResource> keycloakAdminUsername = builder.AddParameter("keycloak-user", "admin", secret: false);
IResourceBuilder<ParameterResource> keycloakAdminPassword = builder.AddParameter("keycloak-password", "admin", secret: true);
IResourceBuilder<KeycloakResource> keycloak = builder
    .AddKeycloak(Resources.Keycloak, 8080, keycloakAdminUsername, keycloakAdminPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

IResourceBuilder<ParameterResource> rabbitMqUsername = builder.AddParameter("rabbitmq-user", "admin", secret: false);
IResourceBuilder<ParameterResource> rabbitMqPassword = builder.AddParameter("rabbitmq-password", "admin", secret: true);
IResourceBuilder<RabbitMQServerResource> rabbitmq = builder.AddRabbitMQ(Resources.RabbitMq, rabbitMqUsername, rabbitMqPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

IResourceBuilder<ParameterResource> redisPassword = builder.AddParameter("redis-password", "admin", secret: true);
IResourceBuilder<RedisResource> redis = builder.AddRedis(Resources.Redis, port: 6379, password: redisPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

builder.AddProject<BookShop_WebApi>("bookshop-webapi")
    .WithReference(postgres).WaitFor(postgres)
    .WithReference(keycloak).WaitFor(keycloak)
    .WithReference(rabbitmq).WaitFor(rabbitmq)
    .WithReference(redis).WaitFor(redis);

await builder.Build().RunAsync();
