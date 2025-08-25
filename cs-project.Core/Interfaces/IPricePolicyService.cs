using cs_project.Core.DTOs;

namespace cs_project.Infrastructure.Services
{
    public interface IPricePolicyService
    {
        Task<IEnumerable<PricePolicyDTO>> GetAllAsync(CancellationToken ct = default);
        Task<PricePolicyDTO?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PricePolicyDTO> CreateAsync(PricePolicyCreateDTO dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(int id, PricePolicyCreateDTO dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}

