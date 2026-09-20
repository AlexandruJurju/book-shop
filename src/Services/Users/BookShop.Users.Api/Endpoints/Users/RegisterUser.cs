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

internal sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (
                UserRegisterRequest request,
                ISender sender,
                CancellationToken cancellationToken
            ) =>
            {
                var registerUserCommand = new RegisterUserCommand(request.Username, request.Email, request.Password);

                Result<Guid> result = await sender.Send(registerUserCommand, cancellationToken);

                return result.ToMinimalApiResult();
            })
            .WithTags(Tags.Users)
            .AllowAnonymous();
    }
}
