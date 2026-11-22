namespace BuildingBlocks.Application.EventBus;

public interface IEventBus
{
    ValueTask PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent;
}
