using System.ComponentModel.DataAnnotations;

namespace BookShop.Cart.Infrastructure.Inbox;

public sealed class InboxJobOptions
{
    public const string ConfigurationSection = nameof(InboxJobOptions);

    [Required]
    public string Cron { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int BatchSize { get; init; }
}
