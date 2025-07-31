
namespace cs_project.Core.Entities
{
    public class Customer: BaseEntity
    {
        public string? FullName { get; set; }
        public string? LoyaltyCardNumber { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = [];
    }
}
