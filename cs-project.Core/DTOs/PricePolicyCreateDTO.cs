using System.ComponentModel.DataAnnotations;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class PricePolicyCreateDTO
    {
        public int? StationId { get; set; }
        [Required] public FuelType FuelType { get; set; }
        [Required] public PricingMethod Method { get; set; }
        [Range(0, double.MaxValue)] public decimal? BaseUsdPrice { get; set; }
        [Range(0, double.MaxValue)] public decimal? MarginPct { get; set; }
        [Range(0, double.MaxValue)] public decimal? MarginRon { get; set; }
        [Range(0, double.MaxValue)] public decimal RoundingIncrement { get; set; }
        [Required] public RoundingMode RoundingMode { get; set; }
        [Required] public DateTime EffectiveFromUtc { get; set; }
        public DateTime? EffectiveToUtc { get; set; }
        public int Priority { get; set; } = 0;
    }
}
