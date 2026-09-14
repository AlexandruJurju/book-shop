namespace BuildingBlocks.Domain;

public interface IAuditableEntity
{
    DateTime CreatedAtUtc { get; set; }
    Guid CreatedByUserId { get; set; }
    DateTime? UpdatedAtUtc { get; set; }
    Guid? UpdatedByUserId { get; set; }
}
