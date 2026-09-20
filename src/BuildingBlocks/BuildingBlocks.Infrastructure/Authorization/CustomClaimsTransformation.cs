using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ardalis.Result;
using BuildingBlocks.Application.Abstractions.Authorization;
using BuildingBlocks.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Authorization;

// cannot be placed only in identity - all services will use claims transformation
public sealed class CustomClaimsTransformation(
    IServiceScopeFactory serviceScopeFactory,
    HybridCache hybridCache
) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub))
        {
            return principal;
        }

        string identityId = principal.GetIdentityId();

        PermissionsResponse permissions = await hybridCache.GetOrCreateAsync(
            $"permissions:{identityId}",
            async cancellationToken =>
            {
                using IServiceScope scope = serviceScopeFactory.CreateScope();

                IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

                Result<PermissionsResponse> result = await permissionService.GetUserPermissionsAsync(identityId);
                return result.IsSuccess
                    ? result.Value
                    : throw new ApplicationException(nameof(IPermissionService.GetUserPermissionsAsync));
            },
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(30),
                LocalCacheExpiration = TimeSpan.FromMinutes(30)
            });

        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, permissions.UserId.ToString()));

        foreach (string permission in permissions.Permissions)
        {
            claimsIdentity.AddClaim(new Claim(CustomClaims.Permission, permission));
        }

        principal.AddIdentity(claimsIdentity);

        return principal;
    }
}
