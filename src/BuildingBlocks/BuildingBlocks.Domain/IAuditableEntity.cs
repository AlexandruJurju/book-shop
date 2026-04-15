using BuildingBlocks.Shared.Helpers;

namespace BuildingBlocks.Domain;

public abstract class AuditableEntity : Entity
{
    public DateTime CreatedAtUtc { get; set; } = DateTimeHelper.UtcNow();
    public DateTime? UpdatedAtUtc { get; set; }
}
