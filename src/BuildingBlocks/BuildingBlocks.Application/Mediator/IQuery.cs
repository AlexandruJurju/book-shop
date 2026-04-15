using Ardalis.Result;
using Mediator;

namespace BuildingBlocks.Application.Mediator;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
