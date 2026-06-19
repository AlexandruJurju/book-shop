namespace BookShop.Users.Application.Users.GetUsers;

public sealed record UserResponse(
    Guid Id,
    string Username,
    string Email
);
