using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities.Pricing
{
    public class StationFuelPrice : BaseEntity
    {
        public int StationId { get; set; }
        public FuelType FuelType { get; set; }
        public decimal PriceRon { get; set; } //published price
        public DateTime EffectiveFromUtc { get; set; } //when pos applied
        public DateTime? EffectiveToUtc { get; set; }
        public int? DerivedFromPolicyId { get; set; }
        public decimal? FxRateUsed { get; set; }
        public decimal? CostRonUsed { get; set; }
        public string? Reason { get; set; }
        public int? CreatedByEmployeeId { get; set; }

        public Station? Station { get; set; }
        public PricePolicy? DerivedFromPolicy { get; set; }
    }
}
