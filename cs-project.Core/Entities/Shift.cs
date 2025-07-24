
namespace cs_project.Core.Entities
{
    public class Shift : BaseEntity
    {
        public int StationId { get; set; }
        public int EmployeeId { get; set; }

        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public decimal TotalSalesAmount { get; set; }

        public Station? Station { get; set; }
        public Employee? Employee { get; set; }
    }
}
