
using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class FuelPriceCreateDTO
    {
        public FuelType FuelType { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
