using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Infrastructure.Outbox;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(OutboxConstants.TableName);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Content)
            .HasMaxLength(2000)
            .HasColumnType("jsonb");

        builder.HasIndex(e => new { e.OccurredOnUtc, e.ProcessedOnUtc })
            .HasDatabaseName("idx_outbox_messages_unprocessed")
            .HasFilter("\"processed_on_utc\" IS NULL")
            .IncludeProperties(e => new { e.Id, e.Type, e.Content });
    }
}
