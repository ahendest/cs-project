using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface ICustomerPaymentRepository
    {
        Task<(IEnumerable<CustomerPayment> Items, int TotalCount)> QueryCustomerPaymentsAsync(PagingQueryParameters query);
        Task<IEnumerable<CustomerPayment>> GetAllAsync();
        Task<CustomerPayment?> GetByIdAsync(int id);
        Task AddAsync(CustomerPayment payment);
        void Update(CustomerPayment payment);
        void Delete(CustomerPayment payment);
        Task<bool> SaveChangesAsync();
    }
}
