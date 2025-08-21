using System.ComponentModel.DataAnnotations;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class CustomerPaymentCreateDTO
    {
        [Required] public int CustomerTransactionId { get; set; }
        [Required] public PaymentMethod Method { get; set; }
        [Range(0, double.MaxValue)] public decimal Amount { get; set; }
        public string? AuthorizationCode { get; set; }
        public DateTime ProcessedAtUtc { get; set; }
    }
}
