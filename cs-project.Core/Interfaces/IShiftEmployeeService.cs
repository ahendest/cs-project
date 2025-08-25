using cs_project.Core.DTOs;

namespace cs_project.Infrastructure.Services
{
    public interface IShiftEmployeeService
    {
        Task<IEnumerable<ShiftEmployeeDTO>> GetEmployeesByShiftIdAsync(int shiftId);
        Task<ShiftEmployeeDTO?> AddEmployeeToShiftAsync(int shiftId, int employeeId);
        Task<bool> RemoveEmployeeFromShiftAsync(int shiftId, int employeeId);
    }
}

