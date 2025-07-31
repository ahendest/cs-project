using cs_project.Core.History;
using static cs_project.Core.Entities.Enums;

public class EmployeeHistory : HistoryBase
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public EmployeeRole Role { get; set; }
    public DateTime HireDateUtc { get; set; }
    public int? StationId { get; set; }
}
