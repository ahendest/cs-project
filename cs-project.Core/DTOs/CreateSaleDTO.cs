using System.ComponentModel.DataAnnotations;

namespace cs_project.Core.DTOs
{
    public class CreateSaleDTO
    {
        [Required] public int PumpId { get; set; }
        [Range(0.0001, 5000)] public decimal Liters { get; set; }
    }
}
