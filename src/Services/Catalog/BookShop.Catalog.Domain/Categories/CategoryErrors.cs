using BuildingBlocks.Shared;

namespace BookShop.Catalog.Domain.Categories;

public static class CategoryErrors
{
    public static Error NotFound(Guid categoryId) =>
        Error.NotFound(
            "Categories.NotFound",
            $"The category with the identifier {categoryId} was not found"
        );
}
