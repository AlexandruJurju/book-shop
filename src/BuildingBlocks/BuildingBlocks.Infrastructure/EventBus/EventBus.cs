using BuildingBlocks.Application.EventBus;
using MassTransit;
using Wolverine;

namespace BuildingBlocks.Infrastructure.EventBus;

public sealed class EventBus(IMessageBus messageBus) : IEventBus
{
    public ValueTask PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
    {
        return messageBus.PublishAsync(integrationEvent);
    }
}
