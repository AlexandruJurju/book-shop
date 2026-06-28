using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IHostApplicationBuilder builder)
    {
        return builder.Services;
    }
}
