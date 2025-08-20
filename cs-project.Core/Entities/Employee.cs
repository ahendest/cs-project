using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class Employee: BaseEntity
    {
        public int? StationId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public EmployeeRole Role { get; set; }
        public DateTime HireDateUtc { get; set; }

        public Station? Station { get; set; }
        public ICollection<ShiftEmployee> ShiftEmployees { get; set; } = [];

    }
}
