namespace BuildingBlocks.Domain;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
}
