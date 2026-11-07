using Ardalis.Result;
using BookShop.Users.Application.Abstractions.Idempotency;
using BookShop.Users.Application.Users.GetUser;
using BookShop.Users.Domain.Users.Events;
using BookShop.Users.Contracts;
using BuildingBlocks.Application.EventBus;
using Mediator;
using Microsoft.Extensions.Logging;

namespace BookShop.Users.Application.Users.RegisterUser;

public sealed class UserRegisteredDomainEventHandler(
    IEventBus bus,
    IMediator mediator,
    IDomainEventConsumerRepository consumerRepository,
    ILogger<UserRegisteredDomainEventHandler> logger
) : IdempotentDomainEventHandler<UserRegisteredDomainEvent>(consumerRepository, logger)
{
    protected override async ValueTask HandleIdempotent(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Result<UserResponse> result = await mediator.Send(new GetUserQuery(notification.UserId), cancellationToken);

        if (!result.IsSuccess)
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
