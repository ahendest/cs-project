using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class SupplierPayment : BaseEntity
    {
        public int FuelDeliveryId { get; set; }
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAtUtc { get; set; }

        public FuelDelivery? FuelDelivery { get; set; }
    }
}
