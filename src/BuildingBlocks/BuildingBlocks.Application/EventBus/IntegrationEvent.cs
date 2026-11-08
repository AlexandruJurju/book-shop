namespace BuildingBlocks.Application.EventBus;

public abstract record IntegrationEvent(
    Guid Id,
    DateTime OccurredAtUtc
) : IIntegrationEvent;
