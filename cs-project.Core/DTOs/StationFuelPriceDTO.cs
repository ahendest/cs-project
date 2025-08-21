using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class StationFuelPriceDTO
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public FuelType FuelType { get; set; }
        public decimal PriceRon { get; set; }
        public DateTime EffectiveFromUtc { get; set; }
        public DateTime? EffectiveToUtc { get; set; }
        public int? DerivedFromPolicyId { get; set; }
        public decimal? FxRateUsed { get; set; }
        public decimal? CostRonUsed { get; set; }
        public string? Reason { get; set; }
        public int? CreatedByEmployeeId { get; set; }
    }
}
