namespace BuildingBlocks.Application.CQRS;

public interface IRequest
{
}

public interface IRequest<TResponse> : IRequest
{
}
