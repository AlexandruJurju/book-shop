using Ardalis.Result;
using Mediator;

namespace BuildingBlocks.Chassis.Application.Mediator;

public interface ICommand : IRequest<Result> { }

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
