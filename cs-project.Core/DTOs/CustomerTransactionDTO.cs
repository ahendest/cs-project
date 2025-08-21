namespace cs_project.Core.DTOs
{
    public class CustomerTransactionDTO
    {
        public int Id { get; set; }
        public int PumpId { get; set; }
        public int StationFuelPriceId { get; set; }
        public decimal Liters { get; set; }
        public decimal PricePerLiter { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset TimestampUtc { get; set; }
    }
}
