using BuildingBlocks.Domain;
using Mediator;

namespace BuildingBlocks.Application.CQRS;

public interface IDomainEventHandler<in TNotification> : INotificationHandler<TNotification>
    where TNotification : IDomainEvent
{
}
