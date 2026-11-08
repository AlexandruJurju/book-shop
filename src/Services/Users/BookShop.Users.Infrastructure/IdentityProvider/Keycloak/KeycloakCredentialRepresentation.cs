namespace BookShop.Users.Infrastructure.IdentityProvider.Keycloak;

internal sealed record KeycloakCredentialRepresentation(
    string Type,
    string Value,
    bool Temporary
);
