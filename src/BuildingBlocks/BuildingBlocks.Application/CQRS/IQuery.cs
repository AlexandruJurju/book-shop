namespace BuildingBlocks.Application.CQRS;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}
