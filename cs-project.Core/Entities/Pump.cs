namespace cs_project.Core.Entities
{
    public class Pump
    {
        public int Id { get; set; }
        public string FuelType { get; set; } = string.Empty;
        public double CurrentVolume { get; set; }
        public string Status { get; set; } = "Idle";
    }
}
