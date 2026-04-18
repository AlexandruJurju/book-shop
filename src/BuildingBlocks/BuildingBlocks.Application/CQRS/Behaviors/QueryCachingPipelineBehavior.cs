using Ardalis.Result;
using Mediator;
using Microsoft.Extensions.Caching.Hybrid;

namespace BuildingBlocks.Application.CQRS.Behaviors;

public sealed class QueryCachingBehavior<TMessage, TResponse>(
    HybridCache hybridCache
) : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : Result
{
    public async ValueTask<TResponse> Handle(TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        if (message is not ICachedQuery cacheable)
        {
            return await next(message, cancellationToken);
        }

        // todo: add distributed cache expiry?
        var entryOptions = new HybridCacheEntryOptions
        {
            LocalCacheExpiration = cacheable.Expiration
        };

        return await hybridCache.GetOrCreateAsync(
            cacheable.Key,
            async ct => await next(message, ct),
            entryOptions,
            cancellationToken: cancellationToken
        );
    }
}
