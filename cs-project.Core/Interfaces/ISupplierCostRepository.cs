using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Interfaces
{
    public interface ISupplierCostRepository
    {
        Task<decimal?> GetWACRonAsync(int stationId, FuelType fuelType, DateTime sinceUtc, CancellationToken ct = default);
    }
}
