using cs_project.Core.Entities;
using cs_project.Core.Models;
using System.Threading;

namespace cs_project.Infrastructure.Repositories
{
    public interface IEmployeeRepository
    {
        Task<(IEnumerable<Employee> Items, int TotalCount)> QueryEmployeesAsync(PagingQueryParameters query, CancellationToken ct);
        Task<IEnumerable<Employee>> GetAllAsync(CancellationToken ct);
        Task<Employee?> GetByIdAsync(int id, CancellationToken ct);
        Task AddAsync(Employee employee, CancellationToken ct);
        void Update(Employee employee);
        void Delete(Employee employee);
        Task<bool> SaveChangesAsync(CancellationToken ct);
    }
}
