namespace cs_project.Core.DTOs
{
    public class SupplierDTO
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? TaxRegistrationNumber { get; set; }
    }
}
