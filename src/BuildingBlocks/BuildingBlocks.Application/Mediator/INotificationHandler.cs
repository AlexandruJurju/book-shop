using Mediator;

namespace BuildingBlocks.Application.Mediator;

public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    ValueTask Handle(TNotification notification, CancellationToken cancellationToken);
}
