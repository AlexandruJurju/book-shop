using Ardalis.Result;
using Mediator;

namespace BuildingBlocks.Application.Mediator;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
