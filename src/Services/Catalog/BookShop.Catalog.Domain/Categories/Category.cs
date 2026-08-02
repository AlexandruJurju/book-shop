using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Categories;

public sealed class Category : Entity, IAuditableEntity, ISoftDeletable
{
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string? Description { get; private set; }
    public Guid? ParentCategoryId { get; private set; }
    public int SortOrder { get; private set; }

    public bool IsDeleted { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
