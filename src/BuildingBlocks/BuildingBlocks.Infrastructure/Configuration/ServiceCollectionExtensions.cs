using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Configuration;

public static class ServiceCollectionExtensions
{
    public static string GetRequiredConnectionString(this IConfiguration configuration, string name)
    {
        return configuration.GetConnectionString(name) ??
            throw new InvalidOperationException($"The connection string {name} was not found");
    }
    
    extension(IServiceCollection services)
    {
        public void ConfigureWithValidation<TOptions>(string section) where TOptions : class
        {
            services
                .AddOptionsWithValidateOnStart<TOptions>()
                .BindConfiguration(section)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}
