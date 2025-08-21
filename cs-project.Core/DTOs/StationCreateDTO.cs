using System.ComponentModel.DataAnnotations;

namespace cs_project.Core.DTOs
{
    public class StationCreateDTO
    {
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Address { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
