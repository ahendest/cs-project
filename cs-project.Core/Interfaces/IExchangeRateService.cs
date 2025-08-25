using cs_project.Core.DTOs;

namespace cs_project.Infrastructure.Services
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<ExchangeRateDTO>> GetAllAsync(CancellationToken ct = default);
        Task<ExchangeRateDTO?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ExchangeRateDTO> CreateAsync(ExchangeRateCreateDTO dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(int id, ExchangeRateCreateDTO dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}

