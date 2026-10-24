using BookShop.Catalog.Domain.Authors;
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

        builder.ComplexProperty(author => author.Name, name =>
        {
            name.Property(personName => personName.Value)
                .HasMaxLength(200)
                .IsRequired();
        });
    }
}
