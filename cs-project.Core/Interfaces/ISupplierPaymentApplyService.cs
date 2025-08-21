using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface ISupplierPaymentApplyService
    {
        Task<PagedResult<SupplierPaymentApplyDTO>> GetSupplierPaymentApplyAsync(PagingQueryParameters query);
        Task<IEnumerable<SupplierPaymentApplyDTO>> GetAllSupplierPaymentAppliesAsync();
        Task<SupplierPaymentApplyDTO?> GetSupplierPaymentApplyByIdAsync(int id);
        Task<SupplierPaymentApplyDTO> CreateSupplierPaymentApplyAsync(SupplierPaymentApplyCreateDTO dto);
        Task<bool> UpdateSupplierPaymentApplyAsync(int id, SupplierPaymentApplyCreateDTO dto);
        Task<bool> DeleteSupplierPaymentApplyAsync(int id);
    }
}
