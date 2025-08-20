
namespace cs_project.Core.Entities
{
    public class Shift : BaseEntity
    {
        public int StationId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public decimal TotalSalesAmount { get; set; }

        public ICollection<ShiftEmployee> ShiftEmployees { get; set; } = [];
        public Station Station { get; set; } = null!;
    }
}
