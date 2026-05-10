namespace BuildingBlocks.Application.CQRS;

public interface ICacheableQuery
{
    string Key { get; }
    TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}

public interface ICacheableQuery<TResponse> : IQuery<TResponse>, ICacheableQuery
{
}
