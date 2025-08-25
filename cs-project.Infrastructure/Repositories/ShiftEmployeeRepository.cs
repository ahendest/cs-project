using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class ShiftEmployeeRepository : IShiftEmployeeRepository
    {
        private readonly AppDbContext _db;

        public ShiftEmployeeRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<ShiftEmployee> Items, int TotalCount)> QueryShiftEmployeesAsync(PagingQueryParameters query)
        {
            var shiftEmployees = _db.ShiftEmployees.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var term = query.SearchTerm.ToLower();
                shiftEmployees = shiftEmployees.Where(se =>
                    se.ShiftId.ToString().Contains(term) ||
                    se.EmployeeId.ToString().Contains(term));
            }

            shiftEmployees = query.SortBy?.ToLower() switch
            {
                "shiftid" => shiftEmployees.OrderBy(se => se.ShiftId),
                "shiftid_desc" => shiftEmployees.OrderByDescending(se => se.ShiftId),
                "employeeid" => shiftEmployees.OrderBy(se => se.EmployeeId),
                "employeeid_desc" => shiftEmployees.OrderByDescending(se => se.EmployeeId),
                _ => shiftEmployees.OrderBy(se => se.Id)
            };

            int totalCount = await shiftEmployees.CountAsync();

            int page = query.Page > 0 ? query.Page : 1;
            int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            shiftEmployees = shiftEmployees.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await shiftEmployees.ToListAsync();
            return (items, totalCount);
        }

        public async Task<IEnumerable<ShiftEmployee>> GetAllAsync() =>
            await _db.ShiftEmployees.AsNoTracking().ToListAsync();

        public async Task<ShiftEmployee?> GetByIdAsync(int id) =>
            await _db.ShiftEmployees.FindAsync(id);

        public async Task AddAsync(ShiftEmployee shiftEmployee) =>
            await _db.ShiftEmployees.AddAsync(shiftEmployee);

        public void Update(ShiftEmployee shiftEmployee) =>
            _db.ShiftEmployees.Update(shiftEmployee);

        public void Delete(ShiftEmployee shiftEmployee) =>
            _db.ShiftEmployees.Remove(shiftEmployee);

        public async Task<bool> SaveChangesAsync() =>
            await _db.SaveChangesAsync() > 0;

        public async Task<IEnumerable<ShiftEmployee>> GetByShiftIdAsync(int shiftId) =>
            await _db.ShiftEmployees.Where(se => se.ShiftId == shiftId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<ShiftEmployee?> GetByShiftAndEmployeeIdAsync(int shiftId, int employeeId) =>
            await _db.ShiftEmployees
                .FirstOrDefaultAsync(se => se.ShiftId == shiftId && se.EmployeeId == employeeId);
    }
}

