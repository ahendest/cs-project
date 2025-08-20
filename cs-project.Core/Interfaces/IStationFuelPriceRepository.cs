using cs_project.Core.Entities.Pricing;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Interfaces
{
    public interface IStationFuelPriceRepository
    {
        // For sales — one price at a specific time and fuel
        Task<StationFuelPrice?> GetCurrentAsync(
            int stationId,
            FuelType fuelType,
            DateTime asOfUtc,
            CancellationToken ct = default);

        // For UI — all current prices (latest per fuel) at a station
        Task<IReadOnlyList<StationFuelPrice>> GetCurrentForStationAsync(
            int stationId,
            DateTime asOfUtc,
            CancellationToken ct = default);

        // Create/publish a price
        Task AddAsync(StationFuelPrice price, CancellationToken ct = default);
    }
}
