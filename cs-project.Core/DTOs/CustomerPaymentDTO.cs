using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class CustomerPaymentDTO
    {
        public int Id { get; set; }
        public int CustomerTransactionId { get; set; }
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public string? AuthorizationCode { get; set; }
        public DateTime ProcessedAtUtc { get; set; }
    }
}
