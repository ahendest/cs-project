using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class Tank : BaseEntity
    {
        public int StationId { get; set; }
        public FuelType FuelType { get; set; }
        public decimal CapacityLiters { get; set; }
        public decimal CurrentVolumeLiters { get; set; }
        public DateTime? LastRefilledAtUtc { get; set; }

        public Station Station { get; set; } = null!;
        public ICollection<Pump> Pumps { get; set; } = [];
    }
}
