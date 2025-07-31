using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class CustomerPayment: BaseEntity
    {
        public int TransactionId { get; set; }
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public string? AuthorizationCode { get; set; }
        public DateTime ProcessedAtUtc { get; set; }

        
        public Transaction? Transaction { get; set; }
    }
}
