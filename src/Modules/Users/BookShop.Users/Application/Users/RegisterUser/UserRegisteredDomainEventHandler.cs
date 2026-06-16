using Ardalis.Result;
using BookShop.Users.Application.Users.GetUser;
using BookShop.Users.Domain.Users.Events;
using BookShop.Users.IntegrationEvents;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Application.EventBus;
using BuildingBlocks.Common.Helpers;
using UserResponse = BookShop.Users.Application.Users.GetUser.UserResponse;

namespace BookShop.Users.Application.Users.RegisterUser;

public sealed class UserRegisteredDomainEventHandler(
    IRequestHandler<GetUserQuery, UserResponse> getUserHandler,
    IEventBus bus
) : DomainEventHandler<UserRegisteredDomainEvent>
{
    public override async Task Handle(UserRegisteredDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        Result<UserResponse> result = await getUserHandler.Handle(
            new GetUserQuery(domainEvent.UserId),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new ApplicationException();
        }

        await bus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                result.Value.Id,
                result.Value.Email
            ),
            cancellationToken);
    }
}
