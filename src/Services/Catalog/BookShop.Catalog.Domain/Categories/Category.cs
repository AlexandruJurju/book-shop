using System;
using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Categories;

public sealed class Category : Entity, IAggregateRoot
{
    public Guid Id { get; private set; }
    public string? Name { get; private set; }
}
