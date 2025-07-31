namespace cs_project.Core.History;

public class TransactionHistory : HistoryBase
{
    public int Id { get; set; }
    public int PumpId { get; set; }
    public int? CustomerId { get; set; }
    public int? EmployeeId { get; set; }
    public int? ShiftId { get; set; }
    public double Liters { get; set; }
    public double PricePerLiter { get; set; }
    public double TotalPrice { get; set; }
    public DateTime TimestampUtc { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

}
