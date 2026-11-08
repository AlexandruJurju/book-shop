namespace BookShop.Users.Infrastructure.IdentityProvider.Keycloak;

internal sealed record KeycloakUser(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    bool EmailVerified,
    bool Enabled,
    KeycloakCredentialRepresentation[] Credentials
);
