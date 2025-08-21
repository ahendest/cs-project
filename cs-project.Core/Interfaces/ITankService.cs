using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface ITankService
    {
        Task<PagedResult<TankDTO>> GetTankAsync(PagingQueryParameters query);
        Task<IEnumerable<TankDTO>> GetAllTanksAsync();
        Task<TankDTO?> GetTankByIdAsync(int id);
        Task<TankDTO> CreateTankAsync(TankCreateDTO dto);
        Task<bool> UpdateTankAsync(int id, TankCreateDTO dto);
        Task<bool> DeleteTankAsync(int id);
    }
}
