using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface ICustomerTransactionRepository
    {
        Task<(IEnumerable<CustomerTransaction> Items, int TotalCount)> QueryCustomerTransactionsAsync(PagingQueryParameters query);
        Task<IEnumerable<CustomerTransaction>> GetAllAsync();
        Task<CustomerTransaction?> GetByIdAsync(int id);
        Task AddAsync(CustomerTransaction transaction);
        void Update(CustomerTransaction transaction);
        void Delete(CustomerTransaction transaction);
        Task<bool> SaveChangesAsync();
    }
}
