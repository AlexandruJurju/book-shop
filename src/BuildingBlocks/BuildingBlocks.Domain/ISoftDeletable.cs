namespace BuildingBlocks.Domain;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    Guid? DeletedBy { get; set; }
}
