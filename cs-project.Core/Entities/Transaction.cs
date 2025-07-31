namespace cs_project.Core.Entities
{
    public class Transaction : BaseEntity
    {
        public int PumpId { get; set; }
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public int? ShiftId { get; set; }
        public double Liters { get; set; }
        public double PricePerLiter { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Timestamp { get; set; }
        public Pump? Pump { get; set; }
        public Customer? Customer { get; set; }
        public Employee? Employee { get; set; }
        public Shift? Shift { get; set; }
        public CustomerPayment? CustomerPayment { get; set; }
    }
}
