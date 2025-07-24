using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class FuelPrice : BaseEntity
    {
        public FuelType FuelType { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
