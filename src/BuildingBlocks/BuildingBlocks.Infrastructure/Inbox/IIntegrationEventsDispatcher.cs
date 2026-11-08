using BuildingBlocks.Application.EventBus;

namespace BuildingBlocks.Infrastructure.Inbox;

public interface IIntegrationEventsDispatcher
{
    Task DispatchAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}
