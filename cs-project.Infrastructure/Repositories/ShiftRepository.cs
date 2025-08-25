using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly AppDbContext _db;

        public ShiftRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<Shift> Items, int TotalCount)> QueryShiftsAsync(PagingQueryParameters query)
        {
            var shifts = _db.Shifts
                .Include(s => s.Station)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                string term = $"%{query.SearchTerm}%";
                shifts = shifts.Where(s =>
                    EF.Functions.Like(s.Station.Name, term) ||
                    EF.Functions.Like(s.StationId.ToString(), term));
            }

            shifts = query.SortBy?.ToLower() switch
            {
                "start" => shifts.OrderBy(s => s.StartUtc),
                "start_desc" => shifts.OrderByDescending(s => s.StartUtc),
                "end" => shifts.OrderBy(s => s.EndUtc),
                "end_desc" => shifts.OrderByDescending(s => s.EndUtc),
                "totalsales" => shifts.OrderBy(s => s.TotalSalesAmount),
                "totalsales_desc" => shifts.OrderByDescending(s => s.TotalSalesAmount),
                _ => shifts.OrderBy(s => s.StartUtc)
            };

            int totalCount = await shifts.CountAsync();

            int page = query.Page > 0 ? query.Page : 1;
            int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            shifts = shifts.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await shifts.ToListAsync();
            return (items, totalCount);
        }

        public async Task<IEnumerable<Shift>> GetAllAsync() =>
            await _db.Shifts.AsNoTracking().ToListAsync();

        public async Task<Shift?> GetByIdAsync(int id) =>
            await _db.Shifts.FindAsync(id);

        public async Task AddAsync(Shift shift) =>
            await _db.Shifts.AddAsync(shift);

        public void Update(Shift shift) =>
            _db.Shifts.Update(shift);

        public void Delete(Shift shift) =>
            _db.Shifts.Remove(shift);

        public async Task<bool> SaveChangesAsync() =>
            await _db.SaveChangesAsync() > 0;
    }
}
