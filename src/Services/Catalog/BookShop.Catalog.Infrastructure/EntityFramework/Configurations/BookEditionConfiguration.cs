using BookShop.Catalog.Domain.Books;
using BookShop.Catalog.Domain.Books.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Catalog.Infrastructure.EntityFramework.Configurations;

public sealed class BookEditionConfiguration : IEntityTypeConfiguration<BookEdition>
{
    public void Configure(EntityTypeBuilder<BookEdition> builder)
    {
        builder.HasKey(edition => edition.Id);

        builder.Property(edition => edition.Id)
            .ValueGeneratedNever();

        builder.Property(edition => edition.Isbn)
            .HasConversion(isbn => isbn.Value, value => Isbn.From(value))
            .HasMaxLength(17);

        builder.Property(edition => edition.PublisherId)
            .IsRequired();

        builder.Property(edition => edition.Format)
            .IsRequired();

        builder.Property(edition => edition.Language)
            .HasConversion(language => language.Code, value => Language.From(value))
            .HasMaxLength(2);

        builder.Property(edition => edition.PublicationDate)
            .IsRequired();

        builder.Property(edition => edition.PageCount)
            .IsRequired();
    }
}
