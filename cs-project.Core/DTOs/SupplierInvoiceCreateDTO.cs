using System.ComponentModel.DataAnnotations;

namespace cs_project.Core.DTOs
{
    public class SupplierInvoiceCreateDTO
    {
        [Required] public int SupplierId { get; set; }
        [Required] public int StationId { get; set; }
        [Required] public DateTime DeliveryDateUtc { get; set; }
        public string Currency { get; set; } = "RON";
        [Range(0, double.MaxValue)] public decimal FxToRon { get; set; } = 1m;
    }
}
