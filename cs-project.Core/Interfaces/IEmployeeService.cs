using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface IEmployeeService
    {
        Task<PagedResult<EmployeeDTO>> GetEmployeeAsync(PagingQueryParameters query);
        Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();
        Task<EmployeeDTO?> GetEmployeeByIdAsync(int id);
        Task<EmployeeDTO> CreateEmployeeAsync(EmployeeCreateDTO dto);
        Task<bool> UpdateEmployeeAsync(int id, EmployeeCreateDTO dto);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
