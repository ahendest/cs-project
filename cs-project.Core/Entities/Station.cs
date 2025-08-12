
namespace cs_project.Core.Entities
{
    public class Station: BaseEntity
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public ICollection<Employee> Employees { get; set; } = [];
        public ICollection<Tank> Tanks { get; set; } = [];
        public ICollection<Pump> Pumps { get; set; } = [];
        public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
        public ICollection<FuelDelivery> FuelDeliveries { get; set; } = [];
    }
}
