using Ardalis.Result;
using Mediator;

namespace BuildingBlocks.Chassis.Application.Mediator;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
