using BuildingBlocks.Infrastructure.Keycloak;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi;

namespace BuildingBlocks.Presentation.OpenApi.Transformers;

internal sealed class SecuritySchemeDefinitionsTransformer(IConfiguration configuration) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        KeycloakOptions keycloakOptions = configuration.GetRequiredSection($"{KeycloakOptions.SectionName}").Get<KeycloakOptions>()!;

        var securityScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Description = "OAuth2 security scheme for the Bookshop",
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    Scopes = new Dictionary<string, string>
                    {
                        { "openid", "openid" },
                        { "profile", "profile" }
                    },
                    AuthorizationUrl = new Uri(keycloakOptions.AuthorizeUrl),
                    TokenUrl = new Uri(keycloakOptions.TokenUrl)
                }
            }
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes.Add(OAuthDefaults.DisplayName, securityScheme);
    }
}
