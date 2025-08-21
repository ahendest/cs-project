using System.ComponentModel.DataAnnotations;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class EmployeeCreateDTO
    {
        public int? StationId { get; set; }
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required] public EmployeeRole Role { get; set; }
        public DateTime HireDateUtc { get; set; }
    }
}
