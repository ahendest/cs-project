using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Transaction>> GetAllAsync() =>
            await _context.Transactions.ToListAsync();

        public async Task<Transaction?> GetByIdAsync(int id) =>
            await _context.Transactions.FindAsync(id);

        public async Task AddAsync(Transaction transaction) =>
            await _context.Transactions.AddAsync(transaction);

        public void Update(Transaction transaction) =>
            _context.Transactions.Update(transaction);

        public void Delete(Transaction transaction) =>
            _context.Transactions.Remove(transaction);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
