using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class Employee: BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public EmployeeRole Role { get; set; }
        public DateTime HireDateUtc { get; set; }
        public int? StationId { get; set; }

        public Station? Station { get; set; }
        public ICollection<Shift> Shifts { get; set; } = [];
        public ICollection<Transaction> Transactions { get; set; } = [];

    }
}
