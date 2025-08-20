using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface IPumpService
    {
        Task<PagedResult<PumpDTO>> GetPumpAsync(PagingQueryParameters query);
        Task<IEnumerable<PumpDTO>> GetAllPumpsAsync();
        Task<PumpDTO?> GetPumpByIdAsync(int id);
        Task<PumpDTO> CreatePumpAsync(PumpCreateDTO pumpDto);
        Task<bool> UpdatePumpAsync(int id, PumpCreateDTO pumpDto);
        Task<bool> DeletePumpAsync(int id);
    }
}
