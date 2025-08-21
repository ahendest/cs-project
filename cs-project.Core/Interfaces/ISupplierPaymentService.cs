using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface ISupplierPaymentService
    {
        Task<PagedResult<SupplierPaymentDTO>> GetSupplierPaymentAsync(PagingQueryParameters query);
        Task<IEnumerable<SupplierPaymentDTO>> GetAllSupplierPaymentsAsync();
        Task<SupplierPaymentDTO?> GetSupplierPaymentByIdAsync(int id);
        Task<SupplierPaymentDTO> CreateSupplierPaymentAsync(SupplierPaymentCreateDTO dto);
        Task<bool> UpdateSupplierPaymentAsync(int id, SupplierPaymentCreateDTO dto);
        Task<bool> DeleteSupplierPaymentAsync(int id);
    }
}
