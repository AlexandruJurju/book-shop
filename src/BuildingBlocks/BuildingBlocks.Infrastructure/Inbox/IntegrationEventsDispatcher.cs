using System.Collections.Concurrent;
using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Inbox;

public sealed class IntegrationEventsDispatcher(
    IServiceProvider serviceProvider
) : IIntegrationEventsDispatcher
{
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public async Task DispatchAsync(
        IEnumerable<IIntegrationEvent> integrationEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (IIntegrationEvent integrationEvent in integrationEvents)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            Type integrationEventType = integrationEvent.GetType();

            Type handlerType = HandlerTypeDictionary.GetOrAdd(
                integrationEventType,
                et => typeof(IIntegrationEventHandler<>).MakeGenericType(et));

            IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

            foreach (object? handler in handlers)
            {
                if (handler is null)
                {
                    continue;
                }

                var handlerWrapper = HandlerWrapper.Create(handler, integrationEventType);
                await handlerWrapper.Handle(integrationEvent, cancellationToken);
            }
        }
    }

    private abstract class HandlerWrapper
    {
        public abstract Task Handle(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);

        public static HandlerWrapper Create(object handler, Type integrationEventType)
        {
            Type wrapperType = WrapperTypeDictionary.GetOrAdd(
                integrationEventType,
                et => typeof(HandlerWrapper<>).MakeGenericType(et));

            return (HandlerWrapper)Activator.CreateInstance(wrapperType, handler)!;
        }
    }

    private sealed class HandlerWrapper<T>(object handler) : HandlerWrapper where T : IIntegrationEvent
    {
        private readonly IIntegrationEventHandler<T> _handler = (IIntegrationEventHandler<T>)handler;

        public override async Task Handle(
            IIntegrationEvent integrationEvent,
            CancellationToken cancellationToken)
        {
            await _handler.HandleAsync((T)integrationEvent, cancellationToken);
        }
    }
}
