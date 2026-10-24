using BookShop.Catalog.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Catalog.Infrastructure.EntityFramework.Configurations;

internal sealed class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> builder)
    {
        builder.HasKey(x => new { x.BookId, x.AuthorId });

        builder.Property(x => x.AuthorId)
            .IsRequired();

        builder.HasOne(x => x.Author)
            .WithMany(x => x.BookAuthors)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Book)
            .WithMany(x => x.BookAuthors)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
