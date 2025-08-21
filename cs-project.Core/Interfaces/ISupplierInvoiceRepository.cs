using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface ISupplierInvoiceRepository
    {
        Task<(IEnumerable<SupplierInvoice> Items, int TotalCount)> QuerySupplierInvoicesAsync(PagingQueryParameters query);
        Task<IEnumerable<SupplierInvoice>> GetAllAsync();
        Task<SupplierInvoice?> GetByIdAsync(int id);
        Task AddAsync(SupplierInvoice invoice);
        void Update(SupplierInvoice invoice);
        void Delete(SupplierInvoice invoice);
        Task<bool> SaveChangesAsync();
    }
}
