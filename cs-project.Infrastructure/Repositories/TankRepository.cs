using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class TankRepository : ITankRepository
    {
        private readonly AppDbContext _context;

        public TankRepository(AppDbContext context) => _context = context;

        public async Task<(IEnumerable<Tank> Items, int TotalCount)> QueryTanksAsync(PagingQueryParameters query)
        {
            var tanks = _context.Tanks
                .Include(t => t.Station)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                string term = $"%{query.SearchTerm}%";
                tanks = tanks.Where(t =>
                    EF.Functions.Like(t.FuelType.ToString(), term) ||
                    EF.Functions.Like(t.Station.Name, term));
            }

            tanks = query.SortBy?.ToLower() switch
            {
                "fueltype_desc" => tanks.OrderByDescending(t => t.FuelType),
                "capacity" => tanks.OrderBy(t => t.CapacityLiters),
                "capacity_desc" => tanks.OrderByDescending(t => t.CapacityLiters),
                _ => tanks.OrderBy(t => t.FuelType)
            };

            int totalCount = await tanks.CountAsync();

            int page = query.Page > 0 ? query.Page : 1;
            int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            tanks = tanks.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await tanks.ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<Tank>> GetAllAsync() =>
            await _context.Tanks.AsNoTracking().ToListAsync();

        public async Task<Tank?> GetByIdAsync(int id) =>
            await _context.Tanks.FindAsync(id);

        public async Task AddAsync(Tank tank) => await _context.Tanks.AddAsync(tank);

        public void Update(Tank tank) => _context.Tanks.Update(tank);

        public void Delete(Tank tank) => _context.Tanks.Remove(tank);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
