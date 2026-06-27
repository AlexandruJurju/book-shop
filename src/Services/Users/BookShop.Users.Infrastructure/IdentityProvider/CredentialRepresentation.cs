namespace BookShop.Users.Infrastructure.IdentityProvider;

internal sealed record CredentialRepresentation(
    string Type,
    string Value,
    bool Temporary
);
