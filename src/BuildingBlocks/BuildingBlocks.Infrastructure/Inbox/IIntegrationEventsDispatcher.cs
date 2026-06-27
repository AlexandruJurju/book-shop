using BuildingBlocks.Application.EventBus;

namespace BuildingBlocks.Infrastructure.Inbox;

public interface IIntegrationEventsDispatcher
{
    Task DispatchAsync(IEnumerable<IIntegrationEvent> integrationEvents, CancellationToken cancellationToken = default);
}
