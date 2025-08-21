using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface IShiftService
    {
        Task<PagedResult<ShiftDTO>> GetShiftAsync(PagingQueryParameters query);
        Task<IEnumerable<ShiftDTO>> GetAllShiftsAsync();
        Task<ShiftDTO?> GetShiftByIdAsync(int id);
        Task<ShiftDTO> CreateShiftAsync(ShiftCreateDTO dto);
        Task<bool> UpdateShiftAsync(int id, ShiftCreateDTO dto);
        Task<bool> DeleteShiftAsync(int id);
    }
}
