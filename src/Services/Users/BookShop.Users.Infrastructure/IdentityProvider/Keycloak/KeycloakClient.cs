using System.Net.Http.Json;

namespace BookShop.Users.Infrastructure.IdentityProvider.Keycloak;

internal sealed class KeyCloakClient(
    HttpClient httpClient
)
{
    internal async Task<string> CreateUserAsync(KeycloakUser keycloakUser, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
            "users",
            keycloakUser,
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
    }

    private static string ExtractIdentityIdFromLocationHeader(
        HttpResponseMessage httpResponseMessage
    )
    {
        const string usersSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header is null");
        }

        int userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        string identityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);

        return identityId;
    }
}
