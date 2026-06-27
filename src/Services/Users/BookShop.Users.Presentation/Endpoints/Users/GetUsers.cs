using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using BookShop.Users.Application.Users.GetUsers;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BookShop.Users.Presentation.Endpoints.Users;

internal sealed class GetUsers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users", async (IQueryHandler<GetUsersQuery, IReadOnlyCollection<UserResponse>> handler, CancellationToken cancellationToken) =>
            {
                var query = new GetUsersQuery();

                Result<IReadOnlyCollection<UserResponse>> result = await handler.HandleAsync(query, cancellationToken);

                return result.ToMinimalApiResult();
            })
            .WithTags(Tags.Users)
            .RequireAuthorization();
    }
}
