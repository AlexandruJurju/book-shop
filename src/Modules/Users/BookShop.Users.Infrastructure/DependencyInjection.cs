using BookShop.Shared.Aspire;
using BookShop.Users.Domain.Users;
using BookShop.Users.Infrastructure.EntityFramework;
using BookShop.Users.Infrastructure.Outbox;
using BookShop.Users.Infrastructure.Users;
using BuildingBlocks.Infrastructure.Cache;
using BuildingBlocks.Infrastructure.Data;
using BuildingBlocks.Infrastructure.Data.EntityFramework.Interceptors;
using BuildingBlocks.Infrastructure.Outbox;
using BuildingBlocks.Infrastructure.Time;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BookShop.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddScoped<IInterceptor, InsertDomainEventsInterceptor>();
        services.AddCustomPostgresDbContext<UsersDbContext>(configuration, UsersResources.Database, Schemas.Users);
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddCustomMemoryCache(configuration);

        services.AddTimeProvider();

        services.AddOutbox<OutboxJob>(configuration);

        return services;
    }
}
