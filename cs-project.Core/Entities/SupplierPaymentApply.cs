namespace cs_project.Core.Entities
{
    public class SupplierPaymentApply : BaseEntity
    {
        public int SupplierPaymentId { get; set; }
        public int SupplierInvoiceId { get; set; }
        public decimal AppliedAmount { get; set; }

        public SupplierPayment? SupplierPayment { get; set; }
        public SupplierInvoice? SupplierInvoice { get; set; }
    }
}
