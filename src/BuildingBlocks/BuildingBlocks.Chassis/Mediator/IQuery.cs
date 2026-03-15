using Ardalis.Result;
using Mediator;

namespace BuildingBlocks.Chassis.Mediator;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
