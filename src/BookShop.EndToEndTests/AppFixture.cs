using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;
using TUnit.Aspire;

namespace BookShop.EndToEndTests;

internal sealed class AppFixture : AspireFixture<Projects.BookShop_WebApi>
{
    protected override TimeSpan ResourceTimeout => TimeSpan.FromMinutes(1);

    protected override void ConfigureBuilder(IDistributedApplicationTestingBuilder builder)
    {
        builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
    }
}
