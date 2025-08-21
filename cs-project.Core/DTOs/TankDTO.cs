using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class TankDTO
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public FuelType FuelType { get; set; }
        public decimal CapacityLiters { get; set; }
        public decimal CurrentVolumeLiters { get; set; }
        public DateTime? LastRefilledAtUtc { get; set; }
    }
}
