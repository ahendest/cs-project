using cs_project.Core.History;

public class ShiftHistory : HistoryBase
{
    public int StationId { get; set; }
    public int EmployeeId { get; set; }

    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public decimal TotalSalesAmount { get; set; }

}