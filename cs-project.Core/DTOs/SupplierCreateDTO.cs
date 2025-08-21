using System.ComponentModel.DataAnnotations;

namespace cs_project.Core.DTOs
{
    public class SupplierCreateDTO
    {
        [Required] public string CompanyName { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        [Phone] public string? Phone { get; set; }
        [EmailAddress] public string? Email { get; set; }
        public string? TaxRegistrationNumber { get; set; }
    }
}
