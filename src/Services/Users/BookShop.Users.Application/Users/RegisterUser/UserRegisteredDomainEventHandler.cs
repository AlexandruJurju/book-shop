using Ardalis.Result;
using BookShop.Users.Application.Users.GetUser;
using BookShop.Users.Domain.Users.Events;
using BookShop.Users.Contracts;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Application.EventBus;
using BuildingBlocks.Common.Extensions;
using Mediator;

namespace BookShop.Users.Application.Users.RegisterUser;

public sealed class UserRegisteredDomainEventHandler(
    IEventBus bus,
    IMediator mediator
) : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public async ValueTask Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Result<UserResponse> result = await mediator.Send(new GetUserQuery(notification.UserId), cancellationToken);

        if (result.IsFailure)
        {
            throw new ApplicationException();
        }

        await bus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                notification.Id,
                notification.OccurredAtUtc,
                result.Value.Id,
                result.Value.Email
            ),
            cancellationToken);
    }
}
