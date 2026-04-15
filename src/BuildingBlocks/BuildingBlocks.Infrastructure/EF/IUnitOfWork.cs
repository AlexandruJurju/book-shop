namespace BuildingBlocks.Infrastructure.EF;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
