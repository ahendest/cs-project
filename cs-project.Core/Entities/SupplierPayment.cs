using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class SupplierPayment : BaseEntity
    {
        public int SupplierId { get; set; } // Should enforce at service level: SupplierPayment.SupplierId == SupplierInvoice.SupplierId

        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAtUtc { get; set; }

        public ICollection<SupplierPaymentApply> Applies { get; set; } = [];
        public Supplier? Supplier { get; set; }
    }
}
