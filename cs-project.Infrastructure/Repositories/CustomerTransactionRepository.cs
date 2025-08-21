using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class CustomerTransactionRepository : ICustomerTransactionRepository
    {
        private readonly AppDbContext _db;

        public CustomerTransactionRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<CustomerTransaction> Items, int TotalCount)> QueryCustomerTransactionsAsync(PagingQueryParameters query)
        {
            var transactions = _db.CustomerTransactions
                .Include(t => t.Pump)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var term = query.SearchTerm.ToLower();
                transactions = transactions.Where(t =>
                    t.PumpId.ToString().Contains(term));
            }

            transactions = query.SortBy?.ToLower() switch
            {
                "date_desc" => transactions.OrderByDescending(t => t.TimestampUtc),
                _ => transactions.OrderBy(t => t.TimestampUtc)
            };

            var totalCount = await transactions.CountAsync();
            var page = query.Page > 0 ? query.Page : 1;
            var pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            var items = await transactions.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<CustomerTransaction>> GetAllAsync() =>
            await _db.CustomerTransactions
                .Include(t => t.Pump)
                .AsNoTracking()
                .ToListAsync();

        public async Task<CustomerTransaction?> GetByIdAsync(int id) =>
            await _db.CustomerTransactions
                .Include(t => t.Pump)
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task AddAsync(CustomerTransaction transaction) =>
            await _db.CustomerTransactions.AddAsync(transaction);

        public void Update(CustomerTransaction transaction) =>
            _db.CustomerTransactions.Update(transaction);

        public void Delete(CustomerTransaction transaction) =>
            _db.CustomerTransactions.Remove(transaction);

        public async Task<bool> SaveChangesAsync() =>
            await _db.SaveChangesAsync() > 0;
    }
}
