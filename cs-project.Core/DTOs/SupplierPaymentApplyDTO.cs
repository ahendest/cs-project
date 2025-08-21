namespace cs_project.Core.DTOs
{
    public class SupplierPaymentApplyDTO
    {
        public int Id { get; set; }
        public int SupplierPaymentId { get; set; }
        public int SupplierInvoiceId { get; set; }
        public decimal AppliedAmount { get; set; }
    }
}
