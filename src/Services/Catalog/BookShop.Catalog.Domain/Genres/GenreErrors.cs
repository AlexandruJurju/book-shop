namespace BookShop.Catalog.Domain.Genres;

public static class GenreErrors
{
    public const string NameRequired = "Genre Name is required.";
    public const string CannotBeOwnParent = "Genre cannot be its own parent.";
}
