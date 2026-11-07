using BookShop.Catalog.Domain.Authors;
using BookShop.Catalog.Domain.Authors.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Catalog.Infrastructure.EntityFramework.Configurations;

public sealed class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(author => author.Id);

        builder.Property(author => author.Id)
            .ValueGeneratedNever();

        builder.Property(author => author.Name)
            .HasConversion(name => name.Value, value => PersonName.From(value))
            .HasMaxLength(PersonName.MaxLength);
    }
}
