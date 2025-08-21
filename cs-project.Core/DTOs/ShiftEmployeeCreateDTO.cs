using System.ComponentModel.DataAnnotations;

namespace cs_project.Core.DTOs
{
    public class ShiftEmployeeCreateDTO
    {
        [Required] public int ShiftId { get; set; }
        [Required] public int EmployeeId { get; set; }
    }
}
