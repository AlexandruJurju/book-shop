using Ardalis.Result;
using BookShop.Users.Application.Abstractions.Idempotency;
using BookShop.Users.Application.Users.GetUser;
using BookShop.Users.Domain.Users.Events;
using BookShop.Users.IntegrationEvents;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Application.EventBus;
using BuildingBlocks.Common.Helpers;
using GetUser_UserResponse = BookShop.Users.Application.Users.GetUser.UserResponse;

namespace BookShop.Users.Application.Users.RegisterUser;

public sealed class UserRegisteredDomainEventHandler(
    IEventBus bus,
    IDomainEventConsumerRepository consumerRepository,
    IQueryHandler<GetUserQuery, GetUser_UserResponse> handler
) : IdempotentDomainEventHandler<UserRegisteredDomainEvent>(consumerRepository)
{
    protected override async Task HandleAsync(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Result<GetUser_UserResponse> result = await handler.HandleAsync(new GetUserQuery(notification.UserId), cancellationToken);

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
