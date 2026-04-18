using BuildingBlocks.Application.CQRS;

namespace BookShop.Catalog.Application.Categories.CreateCategory;

public sealed record CreateCategoryCommand(
    string Name
) : ICommand<Guid>;
