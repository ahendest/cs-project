using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class FuelDelivery : BaseEntity
    {
        public int SupplierId { get; set; }
        public int StationId { get; set; }
        public int TankId { get; set; }

        public FuelType FuelType { get; set; }
        public double QuantityLiters { get; set; }
        public DateTime DeliveryDateUtc { get; set; }
        public string? DeliveryNoteRef { get; set; }

        public Supplier? Supplier { get; set; }
        public Station? Station { get; set; }
        public Tank? Tank { get; set; }
        public SupplierPayment? SupplierPayment { get; set; }
    }
}
