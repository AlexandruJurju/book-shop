using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Categories;

public sealed class Category : Entity, IAggregateRoot
{
    public string? Name { get; private set; }
}
