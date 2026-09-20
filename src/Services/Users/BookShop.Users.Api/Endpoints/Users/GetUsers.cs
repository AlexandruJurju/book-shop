using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using BookShop.Users.Application.Users;
using BookShop.Users.Contracts;
using BuildingBlocks.Api.Endpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BookShop.Users.Api.Endpoints.Users;

internal sealed class GetUsers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users", async (
                IMediator mediator,
                CancellationToken cancellationToken
            ) =>
            {
                var query = new GetUsersQuery();

                Result<IReadOnlyCollection<UserResponse>> result = await mediator.Send(query, cancellationToken);

                return result.ToMinimalApiResult();
            })
            .WithTags(Tags.Users)
            .RequireAuthorization();
    }
}
