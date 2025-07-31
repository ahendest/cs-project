using cs_project.Core.History;
using static cs_project.Core.Entities.Enums;

public class FuelPriceHistory : HistoryBase
{
    public FuelType FuelType { get; set; }
    public double CurrentPrice { get; set; }
    public DateTime UpdatedAt { get; set; }
}