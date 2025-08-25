using cs_project.Core.DTOs;
using cs_project.Core.Models;
using System.Threading;

namespace cs_project.Infrastructure.Services
{
    public interface IEmployeeService
    {
        Task<PagedResult<EmployeeDTO>> GetEmployeeAsync(PagingQueryParameters query, CancellationToken ct);
        Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync(CancellationToken ct);
        Task<EmployeeDTO?> GetEmployeeByIdAsync(int id, CancellationToken ct);
        Task<EmployeeDTO> CreateEmployeeAsync(EmployeeCreateDTO dto, CancellationToken ct);
        Task<bool> UpdateEmployeeAsync(int id, EmployeeCreateDTO dto, CancellationToken ct);
        Task<bool> DeleteEmployeeAsync(int id, CancellationToken ct);
    }
}
