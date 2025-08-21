using System.ComponentModel.DataAnnotations;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class TankCreateDTO
    {
        [Required] public int StationId { get; set; }
        [Required] public FuelType FuelType { get; set; }
        [Range(0, double.MaxValue)] public decimal CapacityLiters { get; set; }
        [Range(0, double.MaxValue)] public decimal CurrentVolumeLiters { get; set; }
        public DateTime? LastRefilledAtUtc { get; set; }
    }
}
