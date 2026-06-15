using Ardalis.Result;
using BookShop.Users.Application.Abstractions.Identity;
using BuildingBlocks.Domain;

namespace BookShop.Users.Domain.Users.Events;

public sealed class UserRegisteredDomainEvent(Guid userid) : DomainEvent, IIdentityProviderService
{
    public Guid UserId { get; init; } = userid;
    public async Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
