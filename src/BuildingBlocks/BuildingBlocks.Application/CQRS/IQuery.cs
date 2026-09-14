using Ardalis.Result;
using Mediator;

namespace BuildingBlocks.Application.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
