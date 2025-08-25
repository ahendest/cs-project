using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class SupplierPaymentApplyRepository : ISupplierPaymentApplyRepository
    {
        private readonly AppDbContext _db;

        public SupplierPaymentApplyRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<SupplierPaymentApply> Items, int TotalCount)> QuerySupplierPaymentAppliesAsync(PagingQueryParameters query)
        {
            var applies = _db.SupplierPaymentApplies.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var term = query.SearchTerm.ToLower();
                applies = applies.Where(a =>
                    a.SupplierPaymentId.ToString().Contains(term) ||
                    a.SupplierInvoiceId.ToString().Contains(term));
            }

            applies = query.SortBy?.ToLower() switch
            {
                "amount_desc" => applies.OrderByDescending(a => a.AppliedAmount),
                "amount" => applies.OrderBy(a => a.AppliedAmount),
                _ => applies.OrderBy(a => a.Id)
            };

            int totalCount = await applies.CountAsync();
            int page = query.Page > 0 ? query.Page : 1;
            int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            applies = applies.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await applies.ToListAsync();
            return (items, totalCount);
        }

        public async Task<IEnumerable<SupplierPaymentApply>> GetAllAsync() =>
            await _db.SupplierPaymentApplies.AsNoTracking().ToListAsync();

        public async Task<SupplierPaymentApply?> GetByIdAsync(int id) =>
            await _db.SupplierPaymentApplies.FindAsync(id);

        public async Task AddAsync(SupplierPaymentApply apply) =>
            await _db.SupplierPaymentApplies.AddAsync(apply);

        public void Update(SupplierPaymentApply apply) =>
            _db.SupplierPaymentApplies.Update(apply);

        public void Delete(SupplierPaymentApply apply) =>
            _db.SupplierPaymentApplies.Remove(apply);

        public async Task<bool> SaveChangesAsync() =>
            await _db.SaveChangesAsync() > 0;
    }
}
