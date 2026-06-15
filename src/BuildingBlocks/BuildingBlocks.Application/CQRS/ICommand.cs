namespace BuildingBlocks.Application.CQRS;

public interface ICommand
{
}

public interface ICommand<TResponse> : ICommand
{
}
