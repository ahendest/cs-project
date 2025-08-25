using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace cs_project.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _db;

        public EmployeeRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<Employee> Items, int TotalCount)> QueryEmployeesAsync(PagingQueryParameters query, CancellationToken ct)
        {
            var employees = _db.Employees
                .Include(e => e.Station)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                string term = $"%{query.SearchTerm}%";
                employees = employees.Where(e =>
                    EF.Functions.Like(e.FirstName, term) ||
                    EF.Functions.Like(e.LastName, term) ||
                    EF.Functions.Like(e.Role.ToString(), term) ||
                    (e.Station != null && EF.Functions.Like(e.Station.Name, term)));
            }

            employees = query.SortBy?.ToLower() switch
            {
                "firstname" => employees.OrderBy(e => e.FirstName),
                "firstname_desc" => employees.OrderByDescending(e => e.FirstName),
                "lastname" => employees.OrderBy(e => e.LastName),
                "lastname_desc" => employees.OrderByDescending(e => e.LastName),
                "role" => employees.OrderBy(e => e.Role),
                "role_desc" => employees.OrderByDescending(e => e.Role),
                "hiredate" => employees.OrderBy(e => e.HireDateUtc),
                "hiredate_desc" => employees.OrderByDescending(e => e.HireDateUtc),
                _ => employees.OrderBy(e => e.LastName)
            };

            int totalCount = await employees.CountAsync(ct);

            int page = query.Page > 0 ? query.Page : 1;
            int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            employees = employees.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await employees.ToListAsync(ct);
            return (items, totalCount);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken ct) =>
            await _db.Employees.AsNoTracking().ToListAsync(ct);

        public async Task<Employee?> GetByIdAsync(int id, CancellationToken ct) =>
            await _db.Employees.FindAsync(new object[] { id }, ct);

        public async Task AddAsync(Employee employee, CancellationToken ct) =>
            await _db.Employees.AddAsync(employee, ct);

        public void Update(Employee employee) =>
            _db.Employees.Update(employee);

        public void Delete(Employee employee) =>
            _db.Employees.Remove(employee);

        public async Task<bool> SaveChangesAsync(CancellationToken ct) =>
            await _db.SaveChangesAsync(ct) > 0;
    }
}
