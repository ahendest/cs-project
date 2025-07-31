using cs_project.Core.History;

public class CustomerHistory : HistoryBase
{
    public string? FullName { get; set; }
    public string? LoyaltyCardNumber { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}
