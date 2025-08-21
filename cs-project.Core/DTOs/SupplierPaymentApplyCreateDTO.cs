using System.ComponentModel.DataAnnotations;

namespace cs_project.Core.DTOs
{
    public class SupplierPaymentApplyCreateDTO
    {
        [Required] public int SupplierPaymentId { get; set; }
        [Required] public int SupplierInvoiceId { get; set; }
        [Range(0, double.MaxValue)] public decimal AppliedAmount { get; set; }
    }
}
