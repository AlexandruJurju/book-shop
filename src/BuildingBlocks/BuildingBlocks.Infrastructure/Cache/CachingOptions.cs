using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Infrastructure.Cache;

public sealed class CachingOptions
{
    public const string SectionName = "Caching";

    [Required]
    // 10 MB
    [Range(1, 10 * 1024 * 1024)]
    public int MaximumPayloadBytes { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int LocalExpirationInMinutes { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int DistributedExpirationInMinutes { get; set; }
}
