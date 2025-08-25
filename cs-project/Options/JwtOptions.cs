using System.ComponentModel.DataAnnotations;

namespace cs_project.Options;

public class JwtOptions
{
    [Required]
    public required string Key { get; set; }

    [Required]
    public required string Issuer { get; set; }

    [Required]
    public required string Audience { get; set; }
}

