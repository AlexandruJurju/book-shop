using System.Security.Claims;
using BuildingBlocks.Application.Identity;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure.Identity;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid? UserId
    {
        get
        {
            string? value = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(value, out Guid guid))
            {
                return guid;
            }

            return null;
        }
    }

    public string? UserName => GetClaimValue(ClaimTypes.Name, x => x);

    public string? Email => GetClaimValue(ClaimTypes.Email, x => x);

    private T? GetClaimValue<T>(string claimType, Func<string, T> converter)
    {
        string? value = httpContextAccessor.HttpContext?.User.FindFirst(claimType)?.Value;

        if (value is not null)
        {
            return converter(value);
        }

        return default;
    }
}
