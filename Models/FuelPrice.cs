namespace cs_project.Models
{
    public class FuelPrice
    {
        public int Id { get; set; }
        public string FuelType { get; set; } = string.Empty;
        public double CurrentPrice { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
