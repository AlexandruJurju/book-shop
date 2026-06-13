using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Infrastructure.Inbox;

public sealed class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable(InboxConstants.TableName);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Content)
            .HasMaxLength(2000)
            .HasColumnType("jsonb");
        
        builder.HasIndex(e => new { e.OccurredOnUtc, e.ProcessedOnUtc })
            .HasDatabaseName("idx_inbox_messages_unprocessed")
            .HasFilter("\"processed_on_utc\" IS NULL")
            .IncludeProperties(e => new { e.Id, e.Type, e.Content });
    }
}
