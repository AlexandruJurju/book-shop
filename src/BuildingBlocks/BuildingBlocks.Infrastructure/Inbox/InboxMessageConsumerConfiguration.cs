using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Infrastructure.Inbox;

public sealed class InboxMessageConsumerConfiguration : IEntityTypeConfiguration<InboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<InboxMessageConsumer> builder)
    {
        builder.ToTable("inbox_message_consumers");

        builder.HasKey(x => new { x.InboxMessageId, x.Name });

        builder.Property(x => x.Name).HasMaxLength(500);
    }
}
