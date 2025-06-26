namespace cs_project.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int PumpId { get; set; }
        public double Liters { get; set; }
        public double PricePerLiter { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Timestamp { get; set; }

        public Pump? Pump { get; set; }
    }
}
