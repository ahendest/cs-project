using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _transactionService;

        public TransactionRepository(AppDbContext transactionService) => _transactionService = transactionService;

        public async Task<(IEnumerable<Transaction> Items, int TotalCount)> QueryTransactionsAsync(PagingQueryParameters query)
        {
            var txs = _transactionService.Transactions.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                string term = query.SearchTerm.ToLower();
                txs = txs.Where(t =>
                    t.PumpId.ToString().Contains(term));
            }

            txs = query.SortBy?.ToLower() switch
            {
                "timestamp" => txs.OrderBy(t => t.Timestamp),
                "timestamp_desc" => txs.OrderByDescending(t => t.Timestamp),
                _ => txs.OrderByDescending(t => t.Timestamp)
            };

            int total = await txs.CountAsync();

            int page = query.Page > 0 ? query.Page : 1;
            int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            txs = txs.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await txs.ToListAsync();

            return (items, total);
        }

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
