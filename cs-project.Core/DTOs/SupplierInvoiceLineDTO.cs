using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class SupplierInvoiceLineDTO
    {
        public int Id { get; set; }
        public int SupplierInvoiceId { get; set; }
        public int TankId { get; set; }
        public FuelType FuelType { get; set; }
        public decimal QuantityLiters { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
