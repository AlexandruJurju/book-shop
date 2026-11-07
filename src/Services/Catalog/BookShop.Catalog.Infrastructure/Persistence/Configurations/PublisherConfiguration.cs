using BookShop.Catalog.Domain.Publishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Catalog.Infrastructure.Persistence.Configurations;

public sealed class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasKey(publisher => publisher.Id);

        builder.Property(publisher => publisher.Id)
            .ValueGeneratedNever();

        builder.Property(publisher => publisher.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
