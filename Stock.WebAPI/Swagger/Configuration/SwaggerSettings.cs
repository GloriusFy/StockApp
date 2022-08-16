using System.ComponentModel.DataAnnotations;

namespace Stock.WebAPI.Swagger.Configuration;

public class SwaggerSettings
{
    [Required, MinLength(1)]
    public string ApiName { get; set; }

    public bool UseSwagger { get; set; }

    [Required, MinLength(1)]
    public string LoginPath { get; set; }
}