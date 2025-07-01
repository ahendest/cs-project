namespace cs_project.Core.DTOs
{
    public class TransactionsDTO
    {
        public int Id { get; set; }
        public int PumpId { get; set; }
        public double Liters { get; set; }
        public double PricePerLiter { get; set; }
        public double TotalPrice { get; set; }
    }
}
