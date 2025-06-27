

namespace cs_project.Core.DTOs
{
    public class PumpCreateDTO
    {
        public string FuelType { get; set; } = string.Empty;
        public double CurrentVolume { get; set; }
        public string Status { get; set; } = "Idle";
    }
}
