using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using BookShop.Users.Application.Users.RegisterUser;
using BuildingBlocks.AspNetCore.Endpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace BookShop.Users.Presentation.Endpoints.Users;

internal sealed class RegisterUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users", Handle)
            .WithTags(Tags.Users)
            .AllowAnonymous();
    }

    private static async Task<IResult> Handle(RegisterUserRequest registerUserRequest, ISender sender, CancellationToken cancellationToken)
    {
        Result<Guid> result = await sender.Send(new RegisterUserCommand(registerUserRequest.Username, registerUserRequest.Email, registerUserRequest.Password), cancellationToken);

        return result.ToMinimalApiResult();
    }

    private sealed record RegisterUserRequest(string Username, string Email, string Password);
}
