using System.ComponentModel.DataAnnotations;

namespace cs_project.Core.DTOs
{
    public class CustomerTransactionCreateDTO
    {
        [Required] public int PumpId { get; set; }
        [Required] public int StationFuelPriceId { get; set; }
        [Range(0.0001, double.MaxValue)] public decimal Liters { get; set; }
        [Range(0, double.MaxValue)] public decimal PricePerLiter { get; set; }
        [Range(0, double.MaxValue)] public decimal TotalPrice { get; set; }
        [Required] public DateTimeOffset TimestampUtc { get; set; }
    }
}
