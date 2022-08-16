using System.ComponentModel.DataAnnotations;

namespace Stock.Infrastructure.Persistence.Settings;

public class ConnectionStrings
{
    [Required, MinLength(1)]
    public string DefaultConnection { get; init; }
}