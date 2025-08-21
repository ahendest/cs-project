using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface IShiftEmployeeService
    {
        Task<PagedResult<ShiftEmployeeDTO>> GetShiftEmployeeAsync(PagingQueryParameters query);
        Task<IEnumerable<ShiftEmployeeDTO>> GetAllShiftEmployeesAsync();
        Task<ShiftEmployeeDTO?> GetShiftEmployeeByIdAsync(int id);
        Task<ShiftEmployeeDTO> CreateShiftEmployeeAsync(ShiftEmployeeCreateDTO dto);
        Task<bool> UpdateShiftEmployeeAsync(int id, ShiftEmployeeCreateDTO dto);
        Task<bool> DeleteShiftEmployeeAsync(int id);
    }
}
