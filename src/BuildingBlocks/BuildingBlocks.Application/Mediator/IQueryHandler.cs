using Ardalis.Result;
using Mediator;

namespace BuildingBlocks.Application.Mediator;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
