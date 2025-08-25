using cs_project.Core.Entities.Pricing;
using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Infrastructure.Repositories
{
    public class StationFuelPriceRepository(AppDbContext db) : IStationFuelPriceRepository
    {
        public Task<StationFuelPrice?> GetCurrentAsync(
            int stationId,
            FuelType fuelType,
            DateTime asOfUtc,
            CancellationToken ct = default)
            => db.StationFuelPrices
                  .AsNoTracking()
                  .Where(p => p.StationId == stationId
                           && p.FuelType == fuelType
                           && p.EffectiveFromUtc <= asOfUtc
                           && (p.EffectiveToUtc == null || p.EffectiveToUtc > asOfUtc)
                           && p.IsActive)
                  .OrderByDescending(p => p.EffectiveFromUtc)
                  .FirstOrDefaultAsync(ct);

        public async Task<IReadOnlyList<StationFuelPrice>> GetCurrentForStationAsync(
            int stationId,
            DateTime asOfUtc,
            CancellationToken ct = default)
        {
            var list = await db.StationFuelPrices
                .AsNoTracking()
                .Where(p => p.StationId == stationId
                         && p.EffectiveFromUtc <= asOfUtc
                         && (p.EffectiveToUtc == null || p.EffectiveToUtc > asOfUtc)
                         && p.IsActive)
                .GroupBy(p => new { p.StationId, p.FuelType })
                .Select(g => g.OrderByDescending(x => x.EffectiveFromUtc).First())
                .ToListAsync(ct);

            return list;
        }

        public async Task<IEnumerable<StationFuelPrice>> GetAllAsync(CancellationToken ct = default) =>
            await db.StationFuelPrices.AsNoTracking().ToListAsync(ct);

        public async Task<StationFuelPrice?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await db.StationFuelPrices.FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task AddAsync(StationFuelPrice price, CancellationToken ct = default)
        {
            await db.StationFuelPrices.AddAsync(price, ct);
        }

        public void Update(StationFuelPrice price) => db.StationFuelPrices.Update(price);

        public void Delete(StationFuelPrice price) => db.StationFuelPrices.Remove(price);

        public async Task<bool> SaveChangesAsync(CancellationToken ct = default) =>
            await db.SaveChangesAsync(ct) > 0;
    }
}

