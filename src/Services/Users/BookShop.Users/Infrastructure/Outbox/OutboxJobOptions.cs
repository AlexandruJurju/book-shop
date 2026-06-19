using System.ComponentModel.DataAnnotations;

namespace BookShop.Users.Infrastructure.Outbox;

public sealed class OutboxJobOptions
{
    public const string ConfigurationSection = nameof(OutboxJobOptions);

    [Required]
    public string Cron { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int BatchSize { get; init; }
}
