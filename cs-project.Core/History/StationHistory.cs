using cs_project.Core.History;

public class StationHistory : HistoryBase
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}