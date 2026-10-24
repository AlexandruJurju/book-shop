using BookShop.Catalog.Domain.Books;
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

        builder.ComplexProperty(
            edition => edition.Isbn,
            isbn =>
            {
                isbn.Property(value => value.Value)
                    .HasMaxLength(17)
                    .IsRequired();
            }
        );

        builder.Property(edition => edition.PublisherId)
            .IsRequired();

        builder.Property(edition => edition.Format)
            .IsRequired();

        builder.ComplexProperty(
            edition => edition.Language,
            language =>
            {
                language.Property(value => value.Code)
                    .HasMaxLength(2)
                    .IsRequired();
            }
        );

        builder.Property(edition => edition.PublicationDate)
            .IsRequired();

        builder.Property(edition => edition.PageCount)
            .IsRequired();
    }
}
