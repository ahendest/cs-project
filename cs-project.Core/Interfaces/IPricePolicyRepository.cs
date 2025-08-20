using cs_project.Core.Entities.Pricing;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Interfaces
{
    public interface IPricePolicyRepository
    {
        Task<IReadOnlyList<PricePolicy>> GetActivePoliciesAsync(int stationId, DateTime asOfUtc, CancellationToken ct = default);
        Task<PricePolicy?> GetFallbackGlobalPolicyAsync(FuelType fuelType, DateTime asOfUtc, CancellationToken ct = default);
    }
}
