using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class PumpRepository : IPumpRepository
    {
        private readonly AppDbContext _pumpService;

        public PumpRepository(AppDbContext pumpService) => _pumpService = pumpService;

        public async Task<(IEnumerable<Pump> Items, int TotalCount)> QueryPumpsAsync(PagingQueryParameters query)
        {
            var pumps = _pumpService.Pumps
                .Include(p => p.Tank)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                string term = query.SearchTerm.ToLower();
                pumps = pumps.Where(p =>
                    p.Tank.FuelType.ToString().ToLower().Contains(term) ||
                    p.Status.ToString().ToLower().Contains(term));
            }

            pumps = query.SortBy?.ToLower() switch
            {
                "fueltype_desc" => pumps.OrderByDescending(p => p.Tank.FuelType),
                "status" => pumps.OrderBy(p => p.Status),
                "status_desc" => pumps.OrderByDescending(p => p.Status),
                _ => pumps.OrderBy(p => p.Tank.FuelType)
            };

            int totalCount = await pumps.CountAsync();

            int page = query.Page > 0 ? query.Page : 1;
            int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            pumps = pumps.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await pumps.ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<Pump>> GetAllAsync() =>
            await _pumpService.Pumps.AsNoTracking().ToListAsync();

        public async Task<Pump?> GetByIdAsync(int id) =>
            await _pumpService.Pumps.FindAsync(id);

        public async Task AddAsync(Pump pump) => await _pumpService.Pumps.AddAsync(pump);

        public void Update(Pump pump) => _pumpService.Pumps.Update(pump);

        public void Delete(Pump pump) => _pumpService.Pumps.Remove(pump);

        public async Task<bool> SaveChangesAsync() => await _pumpService.SaveChangesAsync() > 0;
    }
}
