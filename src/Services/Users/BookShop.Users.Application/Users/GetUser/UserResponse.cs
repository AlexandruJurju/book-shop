namespace BookShop.Users.Application.Users.GetUser;

public sealed record UserResponse(
    Guid Id,
    string Email
);
