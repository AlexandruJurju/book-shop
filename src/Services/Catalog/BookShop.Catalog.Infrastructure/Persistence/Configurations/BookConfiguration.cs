using BookShop.Catalog.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Catalog.Infrastructure.Persistence.Configurations;

internal sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("books");

        builder.HasKey(book => book.Id);

        builder.ComplexProperty(book => book.Title);

        builder.Property(book => book.Description);

        builder.Property(book => book.OriginalPublicationDate);

        builder.Property(book => book.Status)
            .HasConversion<string>();

        builder
            .HasMany(book => book.Authors)
            .WithOne(bookAuthor => bookAuthor.Book)
            .HasForeignKey(bookAuthor => bookAuthor.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(book => book.Editions)
            .WithOne()
            .HasForeignKey("BookId")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(book => book.Genres)
            .WithMany();
    }
}
