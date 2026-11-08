using System.Net;
using Ardalis.Result;
using BookShop.Users.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;

namespace BookShop.Users.Infrastructure.IdentityProvider.Keycloak;

internal sealed class KeycloakIdentityProvider(
    KeyCloakClient keyCloakClient,
    ILogger<KeycloakIdentityProvider> logger
) : IIdentityProvider
{
    private const string PasswordCredentialType = "Password";

    public async Task<Result<string>> CreateUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var userRepresentation = new KeycloakUser(
            user.UserName,
            user.Email,
            string.Empty,
            string.Empty,
            true,
            true,
            [new KeycloakCredentialRepresentation(PasswordCredentialType, user.Password, false)]
        );

        try
        {
            string identityId = await keyCloakClient.CreateUserAsync(userRepresentation, cancellationToken);
            return identityId;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError(exception, "User registration failed");

            return Result.Error("Email is not unique");
        }
    }
}
