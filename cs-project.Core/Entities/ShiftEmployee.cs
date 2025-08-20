
namespace cs_project.Core.Entities
{
    public class ShiftEmployee : BaseEntity
    {
        public int ShiftId { get; set; }
        public int EmployeeId { get; set; }

        public Shift? Shift { get; set; }
        public Employee? Employee { get; set; }
    }
}
