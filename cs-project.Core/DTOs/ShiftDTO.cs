namespace cs_project.Core.DTOs
{
    public class ShiftDTO
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public decimal TotalSalesAmount { get; set; }
    }
}
