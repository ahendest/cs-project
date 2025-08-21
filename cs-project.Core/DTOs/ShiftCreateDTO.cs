using System.ComponentModel.DataAnnotations;

namespace cs_project.Core.DTOs
{
    public class ShiftCreateDTO
    {
        [Required] public int StationId { get; set; }
        [Required] public DateTime StartUtc { get; set; }
        [Required] public DateTime EndUtc { get; set; }
        [Range(0, double.MaxValue)] public decimal TotalSalesAmount { get; set; }
    }
}
