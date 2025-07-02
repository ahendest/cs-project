
namespace cs_project.Core.DTOs
{
    public class FuelPriceCreateDTO
    {
        public string FuelType { get; set; } = string.Empty;
        public double CurrentPrice { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
