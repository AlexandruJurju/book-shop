using BuildingBlocks.Application.CQRS;

namespace BookShop.Users.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
    string UserName,
    string Email,
    string Password
) : ICommand<Guid>;
