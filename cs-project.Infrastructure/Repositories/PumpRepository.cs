using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class PumpRepository : IPumpRepository
    {
        private readonly AppDbContext _context;

        public PumpRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Pump>> GetAllAsync() =>
            await _context.Pumps.AsNoTracking().ToListAsync();

        public async Task<Pump?> GetByIdAsync(int id) =>
            await _context.Pumps.FindAsync(id);

        public async Task AddAsync(Pump pump) => await _context.Pumps.AddAsync(pump);

        public void Update(Pump pump) => _context.Pumps.Update(pump);

        public void Delete(Pump pump) => _context.Pumps.Remove(pump);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
