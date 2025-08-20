using cs_project.Core.Entities.Pricing;
using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Infrastructure.Repositories
{
    public class PricePolicyRepository(AppDbContext db) : IPricePolicyRepository
    {
        public async Task<IReadOnlyList<PricePolicy>> GetActivePoliciesAsync(int stationId, DateTime asOfUtc, CancellationToken ct = default) =>
            await db.PricePolicies
                .Where(p => p.IsActive &&
                            (p.StationId == stationId || p.StationId == null) &&
                            p.EffectiveFromUtc <= asOfUtc &&
                            (p.EffectiveToUtc == null || p.EffectiveToUtc > asOfUtc))
                .OrderByDescending(p => p.StationId.HasValue)
                .ThenByDescending(p => p.Priority)
                .ToListAsync(ct);
        
        public async Task<PricePolicy?> GetFallbackGlobalPolicyAsync(FuelType fuelType, DateTime asOfUtc, CancellationToken ct = default) =>
            await db.PricePolicies
                .Where(p => p.IsActive && p.StationId == null && p.FuelType == fuelType &&
                        p.EffectiveFromUtc <= asOfUtc &&
                        (p.EffectiveToUtc == null || p.EffectiveToUtc > asOfUtc))
                .OrderByDescending(p => p.Priority)
                .FirstOrDefaultAsync(ct);
    }
}
