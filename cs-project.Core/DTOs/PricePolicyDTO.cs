using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class PricePolicyDTO
    {
        public int Id { get; set; }
        public int? StationId { get; set; }
        public FuelType FuelType { get; set; }
        public PricingMethod Method { get; set; }
        public decimal? BaseUsdPrice { get; set; }
        public decimal? MarginPct { get; set; }
        public decimal? MarginRon { get; set; }
        public decimal RoundingIncrement { get; set; }
        public RoundingMode RoundingMode { get; set; }
        public DateTime EffectiveFromUtc { get; set; }
        public DateTime? EffectiveToUtc { get; set; }
        public int Priority { get; set; }
    }
}
