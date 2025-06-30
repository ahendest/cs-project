using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class PumpRepository : IPumpRepository
    {
        private readonly AppDbContext _pumpService;

        public PumpRepository(AppDbContext pumpService) => _pumpService = pumpService;

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
