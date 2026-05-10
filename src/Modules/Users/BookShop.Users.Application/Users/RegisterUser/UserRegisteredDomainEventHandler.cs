using Ardalis.Result;
using BookShop.Users.Application.Abstractions.Idempotency;
using BookShop.Users.Application.Users.GetUser;
using BookShop.Users.Domain.Users.Events;
using BookShop.Users.IntegrationEvents;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Application.EventBus;
using BuildingBlocks.Common.Helpers;
using Mediator;
using UserResponse = BookShop.Users.Application.Users.GetUser.UserResponse;

namespace BookShop.Users.Application.Users.RegisterUser;

public sealed class UserRegisteredDomainEventHandler(
    ISender sender,
    IEventBus bus,
    IIdempotencyDomainEventRepository idempotencyRepository
) : IdempotentDomainEventHandler<UserRegisteredDomainEvent>(idempotencyRepository)
{
    protected override async ValueTask HandleAsync(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Result<UserResponse> result = await sender.Send(
            new GetUserQuery(notification.UserId),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new ApplicationException();
        }

        await bus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                notification.Id,
                notification.OccurredOnUtc,
                result.Value.Id,
                result.Value.Email
            ),
            cancellationToken);
    }
}
