using cs_project.Core.Entities.Pricing;

namespace cs_project.Core.Interfaces
{
    public interface IPricingService
    {
        Task<IReadOnlyList<StationFuelPrice>> RepriceStationAsync(int stationId, DateTime? asOfUtc = null, CancellationToken ct = default);
        Task<IReadOnlyList<StationFuelPrice>> GetCurrentPriceAsync(int stationId, CancellationToken ct = default);
    }
}
