using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class Pump : BaseEntity
    {
        public int StationId { get; set; }
        public int TankId { get; set; }

        public FuelType FuelType { get; set; }
        public double CurrentVolume { get; set; }
        public PumpStatus Status { get; set; } = PumpStatus.Idle;

        
        public Station? Station { get; set; }
        public Tank? Tank { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = [];
    }
}
