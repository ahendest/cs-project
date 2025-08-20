namespace cs_project.Core.Entities
{
    public class SupplierInvoice : BaseEntity
    {
        public int SupplierId { get; set; }
        public int StationId { get; set; }
        public DateTime DeliveryDateUtc { get; set; }
        public string Currency { get; set; } = "RON";
        public decimal FxToRon { get; set; } = 1m;


        public ICollection<SupplierInvoiceLine> Lines { get; set; } = [];
        public Supplier? Supplier { get; set; }
        public Station? Station { get; set; } = null!;
        public ICollection<SupplierPaymentApply> Applies { get; set; } = null!;
    }
}
