using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface ISupplierInvoiceLineService
    {
        Task<PagedResult<SupplierInvoiceLineDTO>> GetSupplierInvoiceLineAsync(PagingQueryParameters query);
        Task<IEnumerable<SupplierInvoiceLineDTO>> GetAllSupplierInvoiceLinesAsync();
        Task<SupplierInvoiceLineDTO?> GetSupplierInvoiceLineByIdAsync(int id);
        Task<IEnumerable<SupplierInvoiceLineDTO>> GetLinesByInvoiceIdAsync(int invoiceId);
        Task<SupplierInvoiceLineDTO> CreateSupplierInvoiceLineAsync(SupplierInvoiceLineCreateDTO dto);
        Task<bool> UpdateSupplierInvoiceLineAsync(int id, SupplierInvoiceLineCreateDTO dto);
        Task<bool> DeleteSupplierInvoiceLineAsync(int id);
    }
}
