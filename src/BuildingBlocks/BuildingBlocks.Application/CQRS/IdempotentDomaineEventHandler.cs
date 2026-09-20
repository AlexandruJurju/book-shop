// using BuildingBlocks.Core;
// using Mediator;
// using Microsoft.Extensions.Logging;
//
// namespace BuildingBlocks.Application.CQRS;
//
// public sealed class IdempotentDomainEventHandler<TDomainEvent>(
//     INotificationHandler<TDomainEvent> decorated,
//     IDomainEventConsumerRepository consumerRepository,
//     ILogger<IdempotentDomainEventHandler<TDomainEvent>> logger
// ) : IDomainEventHandler<TDomainEvent>
//     where TDomainEvent : IDomainEvent
// {
//     public async ValueTask Handle(TDomainEvent notification, CancellationToken cancellationToken)
//     {
//         string consumerName = GetType().Name;
//
//         if (await consumerRepository.ExistsAsync(notification.Id, consumerName, cancellationToken))
//         {
//             logger.LogInformation("Skipping domain event handler {ConsumerName} for domain event {DomainEventId}", consumerName, notification.Id);
//             return;
//         }
//
//         await decorated.Handle(notification, cancellationToken);
//
//         await consumerRepository.AddAsync(notification.Id, consumerName, cancellationToken);
//     }
// }
