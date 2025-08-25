using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class SupplierPaymentRepository : ISupplierPaymentRepository
    {
        private readonly AppDbContext _db;

        public SupplierPaymentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<SupplierPayment> Items, int TotalCount)> QuerySupplierPaymentsAsync(PagingQueryParameters query)
        {
            var payments = _db.SupplierPayments.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var term = query.SearchTerm.ToLower();
                payments = payments.Where(p => p.SupplierId.ToString().Contains(term));
            }

            payments = query.SortBy?.ToLower() switch
            {
                "paidat_desc" => payments.OrderByDescending(p => p.PaidAtUtc),
                "paidat" => payments.OrderBy(p => p.PaidAtUtc),
                _ => payments.OrderBy(p => p.Id)
            };

            int totalCount = await payments.CountAsync();
            int page = query.Page > 0 ? query.Page : 1;
            int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            payments = payments.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await payments.ToListAsync();
            return (items, totalCount);
        }

        public async Task<IEnumerable<SupplierPayment>> GetAllAsync() =>
            await _db.SupplierPayments.AsNoTracking().ToListAsync();

        public async Task<SupplierPayment?> GetByIdAsync(int id) =>
            await _db.SupplierPayments.Include(p => p.Applies).FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(SupplierPayment payment) =>
            await _db.SupplierPayments.AddAsync(payment);

        public void Update(SupplierPayment payment) =>
            _db.SupplierPayments.Update(payment);

        public void Delete(SupplierPayment payment) =>
            _db.SupplierPayments.Remove(payment);

        public async Task<bool> SaveChangesAsync() =>
            await _db.SaveChangesAsync() > 0;
    }
}
