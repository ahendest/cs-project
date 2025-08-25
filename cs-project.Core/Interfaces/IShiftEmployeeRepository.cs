using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface IShiftEmployeeRepository
    {
        Task<(IEnumerable<ShiftEmployee> Items, int TotalCount)> QueryShiftEmployeesAsync(PagingQueryParameters query);
        Task<IEnumerable<ShiftEmployee>> GetAllAsync();
        Task<ShiftEmployee?> GetByIdAsync(int id);
        Task AddAsync(ShiftEmployee shiftEmployee);
        void Update(ShiftEmployee shiftEmployee);
        void Delete(ShiftEmployee shiftEmployee);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<ShiftEmployee>> GetByShiftIdAsync(int shiftId);
        Task<ShiftEmployee?> GetByShiftAndEmployeeIdAsync(int shiftId, int employeeId);
    }
}
