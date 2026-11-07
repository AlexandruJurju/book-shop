using Ardalis.Result;

namespace BuildingBlocks.Application.Abstractions.Authorization;

public interface IPermissionService
{
    Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
}
