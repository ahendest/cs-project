using cs_project.Core.History;
using static cs_project.Core.Entities.Enums;

public class PumpHistory : HistoryBase
{
    public int StationId { get; set; }
    public int TankId { get; set; }

    public FuelType FuelType { get; set; }
    public double CurrentVolume { get; set; }
    public PumpStatus Status { get; set; } = PumpStatus.Idle;
}
