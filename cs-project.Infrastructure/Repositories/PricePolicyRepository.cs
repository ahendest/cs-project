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

        public async Task<IEnumerable<PricePolicy>> GetAllAsync(CancellationToken ct = default) =>
            await db.PricePolicies.AsNoTracking().ToListAsync(ct);

        public async Task<PricePolicy?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await db.PricePolicies.FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task AddAsync(PricePolicy policy, CancellationToken ct = default)
        {
            await db.PricePolicies.AddAsync(policy, ct);
        }

        public void Update(PricePolicy policy) => db.PricePolicies.Update(policy);

        public void Delete(PricePolicy policy) => db.PricePolicies.Remove(policy);

        public async Task<bool> SaveChangesAsync(CancellationToken ct = default) =>
            await db.SaveChangesAsync(ct) > 0;
    }
}

