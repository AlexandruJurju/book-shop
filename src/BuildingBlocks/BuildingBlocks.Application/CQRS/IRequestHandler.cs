using Ardalis.Result;

namespace BuildingBlocks.Application.CQRS;

public interface IRequestHandler<in TRequest>
    where TRequest : IRequest
{
    Task<Result> Handle(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default);
}
