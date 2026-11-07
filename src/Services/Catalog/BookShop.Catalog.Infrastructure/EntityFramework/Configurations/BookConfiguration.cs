using BookShop.Catalog.Domain.Books;
using BookShop.Catalog.Domain.Books.ValueObjects;
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

        builder.Property(b => b.Title)
            .HasConversion(t => t.Value, value => BookTitle.From(value))
            .HasMaxLength(BookTitle.MaxLength);

        builder.Property(book => book.Status);

        builder.HasMany(book => book.Genres)
            .WithMany();
    }
}
