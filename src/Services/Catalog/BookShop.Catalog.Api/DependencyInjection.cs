using System.Reflection;
using BookShop.Catalog.Application;
using BookShop.Catalog.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Catalog.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        return builder.Services;
    }
}
