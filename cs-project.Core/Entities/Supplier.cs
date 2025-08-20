
namespace cs_project.Core.Entities
{
    public class Supplier : BaseEntity
    {
        public required string CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? TaxRegistrationNumber { get; set; }

        public ICollection<SupplierInvoice> Invoices { get; set; } = [];
        public ICollection<SupplierPayment> SupplierPayments { get; set; } = [];
    }
}
