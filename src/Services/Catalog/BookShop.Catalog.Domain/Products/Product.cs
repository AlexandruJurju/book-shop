using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Products;

public sealed class Product : Entity, IAuditableEntity
{
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string? Description { get; private set; }
    public string Sku { get; init; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
