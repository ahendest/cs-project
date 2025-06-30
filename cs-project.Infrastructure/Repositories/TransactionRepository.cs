using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _transactionService;

        public TransactionRepository(AppDbContext transactionService) => _transactionService = transactionService;

        public async Task<IEnumerable<Transaction>> GetAllAsync() =>
            await _transactionService.Transactions.ToListAsync();

        public async Task<Transaction?> GetByIdAsync(int id) =>
            await _transactionService.Transactions.FindAsync(id);

        public async Task AddAsync(Transaction transaction) =>
            await _transactionService.Transactions.AddAsync(transaction);

        public void Update(Transaction transaction) =>
            _transactionService.Transactions.Update(transaction);

        public void Delete(Transaction transaction) =>
            _transactionService.Transactions.Remove(transaction);

        public async Task<bool> SaveChangesAsync() =>
            await _transactionService.SaveChangesAsync() > 0;
    }
}
