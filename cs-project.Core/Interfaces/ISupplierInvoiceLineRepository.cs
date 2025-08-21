using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface ISupplierInvoiceLineRepository
    {
        Task<(IEnumerable<SupplierInvoiceLine> Items, int TotalCount)> QuerySupplierInvoiceLinesAsync(PagingQueryParameters query);
        Task<IEnumerable<SupplierInvoiceLine>> GetAllAsync();
        Task<SupplierInvoiceLine?> GetByIdAsync(int id);
        Task AddAsync(SupplierInvoiceLine line);
        void Update(SupplierInvoiceLine line);
        void Delete(SupplierInvoiceLine line);
        Task<bool> SaveChangesAsync();
    }
}
