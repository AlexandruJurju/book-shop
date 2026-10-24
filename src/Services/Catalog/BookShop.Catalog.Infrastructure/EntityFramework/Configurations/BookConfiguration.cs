using BookShop.Catalog.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Catalog.Infrastructure.EntityFramework.Configurations;

internal sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(book => book.Id);

        builder.Property(book => book.Id)
            .ValueGeneratedNever();

        builder.ComplexProperty(book => book.Title, title =>
        {
            title.Property(bookTitle => bookTitle.Value)
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.Property(book => book.Status);

        builder.HasMany(book => book.Genres)
            .WithMany();
    }
}
