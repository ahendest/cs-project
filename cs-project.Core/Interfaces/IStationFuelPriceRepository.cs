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

        Task<IEnumerable<StationFuelPrice>> GetAllAsync(CancellationToken ct = default);
        Task<StationFuelPrice?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(StationFuelPrice price, CancellationToken ct = default);
        void Update(StationFuelPrice price);
        void Delete(StationFuelPrice price);
        Task<bool> SaveChangesAsync(CancellationToken ct = default);
    }
}

