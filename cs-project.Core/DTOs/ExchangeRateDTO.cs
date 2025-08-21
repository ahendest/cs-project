namespace cs_project.Core.DTOs
{
    public class ExchangeRateDTO
    {
        public int Id { get; set; }
        public string BaseCurrency { get; set; } = "USD";
        public string QuoteCurrency { get; set; } = "RON";
        public decimal Rate { get; set; }
        public DateTime RetrievedAtUtc { get; set; }
        public string? Source { get; set; }
    }
}
