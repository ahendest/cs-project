namespace cs_project.Core.DTOs
{
    public class SupplierInvoiceDTO
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int StationId { get; set; }
        public DateTime DeliveryDateUtc { get; set; }
        public string Currency { get; set; } = "RON";
        public decimal FxToRon { get; set; } = 1m;
    }
}
