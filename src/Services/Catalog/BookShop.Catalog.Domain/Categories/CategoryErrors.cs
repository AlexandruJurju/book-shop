namespace BookShop.Catalog.Domain.Categories;

public static class CategoryErrors
{
    public static string NotFound(Guid categoryId)
    {
        return $"The category with the identifier {categoryId} was not found";
    }
}
