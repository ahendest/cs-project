using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface ISupplierService
    {
        Task<PagedResult<SupplierDTO>> GetSupplierAsync(PagingQueryParameters query);
        Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync();
        Task<SupplierDTO?> GetSupplierByIdAsync(int id);
        Task<SupplierDTO> CreateSupplierAsync(SupplierCreateDTO dto);
        Task<bool> UpdateSupplierAsync(int id, SupplierCreateDTO dto);
        Task<bool> DeleteSupplierAsync(int id);
    }
}
