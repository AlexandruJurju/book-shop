using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using BookShop.Users.Application.Users.RegisterUser;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BookShop.Users.Presentation.Endpoints.Users;

internal sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (
                RegisterUserRequest registerUserRequest,
                ICommandHandler<RegisterUserCommand, Guid> handler,
                CancellationToken cancellationToken
            ) =>
            {
                var registerUserCommand = new RegisterUserCommand(registerUserRequest.Username, registerUserRequest.Email, registerUserRequest.Password);
                Result<Guid> result = await handler.HandleAsync(registerUserCommand, cancellationToken);

                return result.ToMinimalApiResult();
            })
            .WithTags(Tags.Users)
            .AllowAnonymous();
    }

    private sealed record RegisterUserRequest(string Username, string Email, string Password);
}
