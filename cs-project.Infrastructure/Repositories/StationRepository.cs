using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class StationRepository : IStationRepository
    {
        private readonly AppDbContext _db;

        public StationRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<Station> Items, int TotalCount)> QueryStationsAsync(PagingQueryParameters query)
        {
            var stations = _db.Stations
                .Include(s => s.Tanks).ThenInclude(t => t.Pumps)
                .Include(s => s.Employees)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var term = $"%{query.SearchTerm}%";
                stations = stations.Where(s =>
                    EF.Functions.Like(s.Name, term) ||
                    EF.Functions.Like(s.Address, term) ||
                    (s.City != null && EF.Functions.Like(s.City, term)) ||
                    (s.Country != null && EF.Functions.Like(s.Country, term)));
            }

            stations = query.SortBy?.ToLower() switch
            {
                "name_desc" => stations.OrderByDescending(s => s.Name),
                _ => stations.OrderBy(s => s.Name)
            };

            var totalCount = await stations.CountAsync();

            var page = query.Page > 0 ? query.Page : 1;
            var pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            var items = await stations.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<Station>> GetAllAsync() =>
            await _db.Stations
                .Include(s => s.Tanks).ThenInclude(t => t.Pumps)
                .Include(s => s.Employees)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Station?> GetByIdAsync(int id) =>
            await _db.Stations
                .Include(s => s.Tanks).ThenInclude(t => t.Pumps)
                .Include(s => s.Employees)
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task AddAsync(Station station) =>
            await _db.Stations.AddAsync(station);

        public void Update(Station station) =>
            _db.Stations.Update(station);

        public void Delete(Station station) =>
            _db.Stations.Remove(station);

        public async Task<bool> SaveChangesAsync() =>
            await _db.SaveChangesAsync() > 0;
    }
}
