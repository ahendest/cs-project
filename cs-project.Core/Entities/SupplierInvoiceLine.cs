using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class SupplierInvoiceLine : BaseEntity
    {
        public int SupplierInvoiceId { get; set; }
        public int TankId { get; set; }
        public FuelType FuelType { get; set; }
        public decimal QuantityLiters { get; set; }
        public decimal UnitPrice { get; set; }

        public SupplierInvoice SupplierInvoice { get; set; } = null!;
        public Tank Tank { get; set; } = null!;
    }
}
