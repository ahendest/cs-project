using System.ComponentModel.DataAnnotations;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class StationFuelPriceCreateDTO
    {
        [Required] public int StationId { get; set; }
        [Required] public FuelType FuelType { get; set; }
        [Range(0, double.MaxValue)] public decimal PriceRon { get; set; }
        [Required] public DateTime EffectiveFromUtc { get; set; }
        public DateTime? EffectiveToUtc { get; set; }
        public int? DerivedFromPolicyId { get; set; }
        [Range(0, double.MaxValue)] public decimal? FxRateUsed { get; set; }
        [Range(0, double.MaxValue)] public decimal? CostRonUsed { get; set; }
        public string? Reason { get; set; }
        public int? CreatedByEmployeeId { get; set; }
    }
}
