using Ardalis.Result;
using BookShop.Users.Infrastructure.EntityFramework;
using BuildingBlocks.Application.Abstractions.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Users.Infrastructure.Authorization;

internal sealed class PermissionService(
    UsersDbContext usersDbContext
) : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        PermissionsResponse? result = await usersDbContext.Users
            .Where(user => user.IdentityId == identityId)
            .Select(user => new PermissionsResponse(
                user.Id,
                user.Roles
                    .SelectMany(role => role.Permissions)
                    .Select(p => p.Name)
                    .ToHashSet()
            ))
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return Result.NotFound("User not found");
        }

        return Result.Success(result);
    }
}
