using Ardalis.Result;
using Mediator;

namespace BuildingBlocks.Application.CQRS;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
