using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Infrastructure.Cache;

[OptionsValidator]
public sealed partial class CacheOptions : IValidateOptions<CacheOptions>
{
    public const string ConfigurationSection = "Caching";

    [Required]
    [Range(1, int.MaxValue)]
    public int MaximumPayloadBytes { get; set; }

    [Required]
    public TimeSpan Expiration { get; set; }
}
