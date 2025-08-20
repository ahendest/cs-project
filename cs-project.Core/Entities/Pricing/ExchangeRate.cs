
namespace cs_project.Core.Entities.Pricing
{
    public class ExchangeRate : BaseEntity
    {
        public required string BaseCurrency { get; set; } = "USD";
        public required string QuoteCurrency { get; set; } = "RON";
        public decimal Rate { get; set; }
        public DateTime RetrievedAtUtc { get; set; }
        public string? Source { get; set; }
    }
}
