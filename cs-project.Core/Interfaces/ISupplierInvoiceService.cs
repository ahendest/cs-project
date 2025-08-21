using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface ISupplierInvoiceService
    {
        Task<PagedResult<SupplierInvoiceDTO>> GetSupplierInvoiceAsync(PagingQueryParameters query);
        Task<IEnumerable<SupplierInvoiceDTO>> GetAllSupplierInvoicesAsync();
        Task<SupplierInvoiceDTO?> GetSupplierInvoiceByIdAsync(int id);
        Task<SupplierInvoiceDTO> CreateSupplierInvoiceAsync(SupplierInvoiceCreateDTO dto);
        Task<bool> UpdateSupplierInvoiceAsync(int id, SupplierInvoiceCreateDTO dto);
        Task<bool> DeleteSupplierInvoiceAsync(int id);
    }
}
