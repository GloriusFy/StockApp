using System.ComponentModel.DataAnnotations;

namespace Stock.Infrastructure.Persistence.Settings;

public class ApplicationDbSettings
{
    [Required]
    public bool? AutoMigrate { get; init; }
    [Required]
    public bool? AutoSeed { get; init; }
}