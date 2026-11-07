namespace BookShop.Catalog.Domain.Books;

public static class BookErrors
{
    public const string NoAuthor = "A book must have at least one author.";
    public const string AuthorAlreadyExists = "Book already contains the author.";
    public const string AuthorNotFound = "Author not found for the book.";
    public const string GenreAlreadyExists = "Book already contains the genre.";
    public const string GenreNotFound = "Book doesn't contain the genre.";

    public static string EditionAlreadyExists(string format) => $"An edition in {format} format already exists for this book.";
}
