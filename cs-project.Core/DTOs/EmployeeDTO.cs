using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public int? StationId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public EmployeeRole Role { get; set; }
        public DateTime HireDateUtc { get; set; }
    }
}
