using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Infrastructure.Outbox;

public sealed class OutboxJobOptions
{
    public const string ConfigurationSection = "OutboxJob";

    [Required]
    public string Cron { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int BatchSize { get; init; }
}
