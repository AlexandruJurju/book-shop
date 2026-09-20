using BookShop.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Users.Infrastructure.EntityFramework.Users;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.UserName)
            .HasMaxLength(200);

        builder.ComplexProperty(user => user.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("email")
                .HasMaxLength(254)
                .IsRequired();
        });

        builder.HasIndex(user => user.Email).IsUnique();

        builder.HasIndex(u => u.IdentityId).IsUnique();
    }
}
