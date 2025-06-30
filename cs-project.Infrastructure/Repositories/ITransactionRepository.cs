using cs_project.Core.Entities;

namespace cs_project.Infrastructure.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int id);
        Task AddAsync(Transaction transaction);
        void Update(Transaction transaction);
        void Delete(Transaction transaction);
        Task<bool>SaveChangesAsync();
    }
}
