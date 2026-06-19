using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using BookShop.Users.Application.Users.RegisterUser;
using BuildingBlocks.Presentation.Endpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace BookShop.Users.Presentation.Endpoints.Users;

internal sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (RegisterUserRequest registerUserRequest, ISender sender, CancellationToken cancellationToken) =>
            {
                Result<Guid> result =
                    await sender.Send(new RegisterUserCommand(registerUserRequest.Username, registerUserRequest.Email, registerUserRequest.Password),
                        cancellationToken);

                return result.ToMinimalApiResult();
            })
            .WithTags(Tags.Users)
            .AllowAnonymous();
    }

    private sealed record RegisterUserRequest(string Username, string Email, string Password);
}
