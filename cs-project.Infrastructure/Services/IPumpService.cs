using cs_project.Core.DTOs;

namespace cs_project.Infrastructure.Services
{
    public interface IPumpService
    {
        Task<IEnumerable<PumpDTO>> GetAllPumpsAsync();
        Task<PumpDTO?> GetPumpByIdAsync(int id);
        Task<PumpDTO> CreatePumpAsync(PumpCreateDTO pumpDto);
        Task<bool> UpdatePumpAsync(int id, PumpCreateDTO pumpDto);
        Task<bool> DeletePumpAsync(int id);
    }
}
