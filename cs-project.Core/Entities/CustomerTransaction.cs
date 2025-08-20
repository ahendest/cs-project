using cs_project.Core.Entities.Pricing;

namespace cs_project.Core.Entities
{
    public class CustomerTransaction : BaseEntity
    {
        public int PumpId { get; set; }
        public int StationFuelPriceId { get; set; }  

        public decimal Liters { get; set; }
        public decimal PricePerLiter { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset TimestampUtc { get; set; }

        public Pump Pump { get; set; } = null!;
        public StationFuelPrice StationFuelPrice { get; set; } = null!;
        public ICollection<CustomerPayment> CustomerPayments { get; set; } = [];
    }
}
