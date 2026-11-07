using Microsoft.Extensions.Hosting;

namespace BookShop.Users.Application;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        return builder;
    }
}
