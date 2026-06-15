using System.Net;
using Ardalis.Result;
using BookShop.Users.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;

namespace BookShop.Users.Infrastructure.IdentityProvider;

internal sealed class KeycloakIdentityProviderService(
    KeyCloakClient keyCloakClient,
    ILogger<KeycloakIdentityProviderService> logger
) : IIdentityProviderService
{
    private const string PasswordCredentialType = "Password";

    public async Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var userRepresentation = new UserRepresentation(
            user.UserName,
            user.Email,
            string.Empty,
            string.Empty,
            true,
            true,
            [new CredentialRepresentation(PasswordCredentialType, user.Password, false)]
        );

        try
        {
            string identityId = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);
            return identityId;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError(exception, "User registration failed");

            return Result.Error("Email is not unique");
        }
    }
}
