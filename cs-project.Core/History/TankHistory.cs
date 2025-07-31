using cs_project.Core.History;
using static cs_project.Core.Entities.Enums;

public class TankHistory : HistoryBase
{
    public int StationId { get; set; }
    public FuelType FuelType { get; set; }
    public double CapacityLiters { get; set; }
    public double CurrentVolumeLiters { get; set; }
    public DateTime? LastRefilledAtUtc { get; set; }

}