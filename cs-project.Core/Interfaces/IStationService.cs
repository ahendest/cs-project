using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface IStationService
    {
        Task<PagedResult<StationDTO>> GetStationAsync(PagingQueryParameters query, CancellationToken ct);
        Task<IEnumerable<StationDTO>> GetAllStationsAsync(CancellationToken ct);
        Task<StationDTO?> GetStationByIdAsync(int id, CancellationToken ct);
        Task<StationDTO> CreateStationAsync(StationCreateDTO dto, CancellationToken ct);
        Task<bool> UpdateStationAsync(int id, StationCreateDTO dto, CancellationToken ct);
        Task<bool> DeleteStationAsync(int id, CancellationToken ct);
    }
}
