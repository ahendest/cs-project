using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class SupplierPaymentDTO
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAtUtc { get; set; }
    }
}
