using cs_project.Core.DTOs;

namespace cs_project.Infrastructure.Services
{
    public interface IStationFuelPriceService
    {
        Task<IEnumerable<StationFuelPriceDTO>> GetAllAsync(CancellationToken ct = default);
        Task<StationFuelPriceDTO?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<StationFuelPriceDTO> CreateAsync(StationFuelPriceCreateDTO dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(int id, StationFuelPriceCreateDTO dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}

