using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class Tank : BaseEntity
    {
        public int StationId { get; set; }
        public FuelType FuelType { get; set; }
        public double CapacityLiters { get; set; }
        public double CurrentVolumeLiters { get; set; }
        public DateTime? LastRefilledAtUtc { get; set; }

        public Station? Station { get; set; }
        public ICollection<Pump> Pumps { get; set; } = [];
        public ICollection<FuelDelivery> FuelDeliveries { get; set; } = [];
    }
}
