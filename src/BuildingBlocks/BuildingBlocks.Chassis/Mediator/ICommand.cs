using Ardalis.Result;
using Mediator;

namespace BuildingBlocks.Chassis.Mediator;

public interface ICommand : IRequest<Result> { }

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
