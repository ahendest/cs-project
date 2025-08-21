using System.ComponentModel.DataAnnotations;

namespace cs_project.Core.DTOs
{
    public class ExchangeRateCreateDTO
    {
        [Required] public string BaseCurrency { get; set; } = "USD";
        [Required] public string QuoteCurrency { get; set; } = "RON";
        [Range(0, double.MaxValue)] public decimal Rate { get; set; }
        public DateTime RetrievedAtUtc { get; set; }
        public string? Source { get; set; }
    }
}
