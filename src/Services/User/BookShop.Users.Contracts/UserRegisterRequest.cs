namespace BookShop.Users.Contracts;

public sealed record UserRegisterRequest(
    string Username,
    string Email,
    string Password
);
