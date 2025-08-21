using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _db;

        public SupplierRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<Supplier> Items, int TotalCount)> QuerySuppliersAsync(PagingQueryParameters query)
        {
            var suppliers = _db.Suppliers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                string term = query.SearchTerm.ToLower();
                suppliers = suppliers.Where(s =>
                    s.CompanyName.ToLower().Contains(term) ||
                    (s.ContactPerson != null && s.ContactPerson.ToLower().Contains(term)) ||
                    (s.Phone != null && s.Phone.ToLower().Contains(term)) ||
                    (s.Email != null && s.Email.ToLower().Contains(term)) ||
                    (s.TaxRegistrationNumber != null && s.TaxRegistrationNumber.ToLower().Contains(term)));
            }

            suppliers = query.SortBy?.ToLower() switch
            {
                "companyname_desc" => suppliers.OrderByDescending(s => s.CompanyName),
                "contactperson" => suppliers.OrderBy(s => s.ContactPerson),
                "contactperson_desc" => suppliers.OrderByDescending(s => s.ContactPerson),
                _ => suppliers.OrderBy(s => s.CompanyName)
            };

            int totalCount = await suppliers.CountAsync();

            int page = query.Page > 0 ? query.Page : 1;
            int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            suppliers = suppliers.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await suppliers.ToListAsync();
            return (items, totalCount);
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync() =>
            await _db.Suppliers.AsNoTracking().ToListAsync();

        public async Task<Supplier?> GetByIdAsync(int id) =>
            await _db.Suppliers.FindAsync(id);

        public async Task AddAsync(Supplier supplier) =>
            await _db.Suppliers.AddAsync(supplier);

        public void Update(Supplier supplier) =>
            _db.Suppliers.Update(supplier);

        public void Delete(Supplier supplier) =>
            _db.Suppliers.Remove(supplier);

        public async Task<bool> SaveChangesAsync() =>
            await _db.SaveChangesAsync() > 0;
    }
}
