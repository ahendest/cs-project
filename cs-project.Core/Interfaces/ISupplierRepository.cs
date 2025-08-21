using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface ISupplierRepository
    {
        Task<(IEnumerable<Supplier> Items, int TotalCount)> QuerySuppliersAsync(PagingQueryParameters query);
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task AddAsync(Supplier supplier);
        void Update(Supplier supplier);
        void Delete(Supplier supplier);
        Task<bool> SaveChangesAsync();
    }
}
