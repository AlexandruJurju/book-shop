using BuildingBlocks.Infrastructure.Keycloak;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace BuildingBlocks.Presentation.Scalar;

public static class WebApplicationExtensions
{
    public static void MapCustomScalar(
        this WebApplication app,
        IConfiguration configuration
    )
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        KeycloakOptions keycloak = configuration
            .GetRequiredSection(KeycloakOptions.SectionName)
            .Get<KeycloakOptions>()!;

        app.MapOpenApi();

        app.MapScalarApiReference(options =>
        {
            options
                .AddPreferredSecuritySchemes(OAuthDefaults.DisplayName)
                .AddAuthorizationCodeFlow(
                    OAuthDefaults.DisplayName,
                    flow =>
                    {
                        flow.WithPkce(Pkce.Sha256)
                            .WithClientId(keycloak.PublicClientId)
                            .AddBodyParameter("audience", "account");

                        flow.SelectedScopes = ["openid", "profile"];
                    }
                );
        });
    }
}
