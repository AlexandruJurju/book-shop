using Ardalis.Result;

namespace BookShop.Users.Application.Abstractions.Identity;

public interface IIdentityProvider
{
    Task<Result<string>> CreateUserAsync(UserModel user, CancellationToken cancellationToken = default);
}
