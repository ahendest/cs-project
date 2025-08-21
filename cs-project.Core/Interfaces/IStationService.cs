using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface IStationService
    {
        Task<PagedResult<StationDTO>> GetStationAsync(PagingQueryParameters query);
        Task<IEnumerable<StationDTO>> GetAllStationsAsync();
        Task<StationDTO?> GetStationByIdAsync(int id);
        Task<StationDTO> CreateStationAsync(StationCreateDTO dto);
        Task<bool> UpdateStationAsync(int id, StationCreateDTO dto);
        Task<bool> DeleteStationAsync(int id);
    }
}
