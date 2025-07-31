using cs_project.Core.History;
using static cs_project.Core.Entities.Enums;

public class FuelDeliveryHistory : HistoryBase
{
    public int SupplierId { get; set; }
    public int StationId { get; set; }
    public int TankId { get; set; }

    public FuelType FuelType { get; set; }
    public double QuantityLiters { get; set; }
    public DateTime DeliveryDateUtc { get; set; }
    public string? DeliveryNoteRef { get; set; }
}
