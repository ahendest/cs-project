using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface ITransactionRepository
    {
        Task<(IEnumerable<Transaction> Items, int TotalCount)> QueryTransactionsAsync(PagingQueryParameters query);
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int id);
        Task AddAsync(Transaction transaction);
        void Update(Transaction transaction);
        void Delete(Transaction transaction);
        Task<bool>SaveChangesAsync();
    }
}
