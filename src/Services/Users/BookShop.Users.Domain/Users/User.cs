using BookShop.Users.Domain.Users.Events;
using BuildingBlocks.Domain;

namespace BookShop.Users.Domain.Users;

public sealed class User : AggregateRoot
{
    // For EF Core
    private User() { }

    private User(Guid id, string userName, string email, string identityId)
    {
        Id = id;
        UserName = userName;
        Email = email;
        IdentityId = identityId;
    }

    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string IdentityId { get; private set; }

    private readonly List<Role> _roles = new();
    public IReadOnlyCollection<Role> Roles => [.. _roles];

    public static User Create(string userName, string email, string identityId)
    {
        var user = new User(Guid.CreateVersion7(), userName, email, identityId);

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id));

        user._roles.Add(Role.Registered);

        return user;
    }
}
