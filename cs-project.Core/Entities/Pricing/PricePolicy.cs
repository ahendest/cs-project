
using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities.Pricing
{
    public class PricePolicy : BaseEntity
    {
        public int? StationId { get; set; } // null -> global price
        public FuelType FuelType { get; set; }
        public PricingMethod Method { get; set; }
        public decimal? BaseUsdPrice { get; set; }
        public decimal? MarginPct { get; set; }
        public decimal? MarginRon { get; set; }
        public decimal RoundingIncrement { get; set; } = 0.01m;
        public RoundingMode RoundingMode { get; set; } = RoundingMode.Round;
        public DateTime EffectiveFromUtc { get; set; } = DateTime.UtcNow;
        public DateTime? EffectiveToUtc { get; set; }
        public int Priority { get; set; } = 0;
        
        public Station? Station { get; set; }

    }
}
