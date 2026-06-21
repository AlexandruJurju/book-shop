using BookShop.Users.Domain.Users;
using BookShop.Users.Domain.Users.Events;
using BookShop.Users.UnitTests.Abstractions;
using TUnit.Assertions.Should;
using TUnit.Assertions.Should.Extensions;

namespace BookShop.Users.UnitTests.Users;

internal sealed class UserTests : BaseTest
{
    [Test]
    [DisplayName("Registering a user should raise a domain event")]
    public async Task Registering_User_Raises_Domain_Event()
    {
        var user = User.Create(
            Faker.Internet.UserName(),
            Faker.Internet.Email(),
            Guid.NewGuid().ToString()
        );

        UserRegisteredDomainEvent domainEvent = AssertDomainEventWasPublished<UserRegisteredDomainEvent>(user);

        await domainEvent.UserId.Should().BeEqualTo(user.Id);
    }

    [Test]
    [DisplayName("Registered user should have only Registered Role")]
    public async Task Registering_User_Assigns_Registered_Role()
    {
        var user = User.Create(
            Faker.Internet.UserName(),
            Faker.Internet.Email(),
            Guid.NewGuid().ToString()
        );

        await user.Roles.Count.Should().BeEqualTo(1);
        await user.Roles.SingleOrDefault().Should().BeEqualTo(Role.Registered);
    }
}
