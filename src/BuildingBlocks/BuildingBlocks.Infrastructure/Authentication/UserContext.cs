using BuildingBlocks.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure.Authentication;

internal sealed class UserContext(
    IHttpContextAccessor httpContextAccessor
) : IUserContext
{
    public Guid UserId => httpContextAccessor
            .HttpContext?
            .User
            .GetUserId()
        ?? throw new InvalidOperationException("User identity is not available");

    public string IdentityId => httpContextAccessor
            .HttpContext?
            .User
            .GetIdentityId()
        ?? throw new InvalidOperationException("User identity is not available");
}
