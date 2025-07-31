using cs_project.Core.History;
using static cs_project.Core.Entities.Enums;

public class CustomerPaymentHistory : HistoryBase
{
    public int TransactionId { get; set; }
    public PaymentMethod Method { get; set; }
    public decimal Amount { get; set; }
    public string? AuthorizationCode { get; set; }
    public DateTime ProcessedAtUtc { get; set; }
}
