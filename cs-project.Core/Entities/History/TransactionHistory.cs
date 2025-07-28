namespace cs_project.Core.Entities.History;

public class TransactionHistory
{
    // Original business fields
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

    // Audit metadata
    public long AuditId { get; set; } // PK
    public int AuditUserId { get; set; }
    public DateTime AuditTsUtc { get; set; }
    public char AuditOp { get; set; } // 'U' or 'D'
    public Guid AuditCorrelation { get; set; }
}
