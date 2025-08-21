using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface IEmployeeRepository
    {
        Task<(IEnumerable<Employee> Items, int TotalCount)> QueryEmployeesAsync(PagingQueryParameters query);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);
        Task<bool> SaveChangesAsync();
    }
}
