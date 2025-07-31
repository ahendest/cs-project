using cs_project.Core.History;
using static cs_project.Core.Entities.Enums;

public class SupplierPaymentHistory : HistoryBase
{
    public int FuelDeliveryId { get; set; }
    public PaymentMethod Method { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAtUtc { get; set; }
}