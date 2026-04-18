using BookShop.Users.Domain.Users.Events;
using BuildingBlocks.Common.Helpers;
using BuildingBlocks.Domain;

namespace BookShop.Users.Domain.Users;

public sealed class User : Entity, IAggregateRoot
{
    // For EF Core
    private User()
    {
    }

    private User(Guid id, UserName userName, Email email)
        : base(id)
    {
        UserName = userName;
        Email = email;
    }

    public UserName UserName { get; private set; }
    public Email Email { get; private set; }

    public static User Create(UserName userName, Email email)
    {
        var user = new User(GuidHelper.NewGuid(), userName, email);

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id));

        return user;
    }
}
