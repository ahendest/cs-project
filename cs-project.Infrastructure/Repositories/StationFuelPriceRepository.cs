using cs_project.Core.Entities.Pricing;
using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task AddAsync(StationFuelPrice price, CancellationToken ct = default)
        {
            await db.StationFuelPrices.AddAsync(price, ct);
            await db.SaveChangesAsync(ct);
        }

        
    }
}