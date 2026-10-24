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

        builder.HasMany(book => book.Authors)
            .WithOne(bookAuthor => bookAuthor.Book)
            .HasForeignKey(bookAuthor => bookAuthor.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
