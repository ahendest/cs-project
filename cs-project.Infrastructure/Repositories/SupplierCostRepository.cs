using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Infrastructure.Repositories
{
    public class SupplierCostRepository(AppDbContext db) : ISupplierCostRepository
    {
        public async Task<decimal?> GetWACRonAsync(int stationId, FuelType fuelType, DateTime sinceUtc, CancellationToken ct = default)
        {
            var q = from line in db.SupplierInvoiceLines
                    join inv in db.SupplierInvoices on line.SupplierInvoiceId equals inv.Id
                    where inv.StationId == stationId
                        && inv.DeliveryDateUtc >= sinceUtc
                        && line.FuelType == fuelType
                        && line.IsActive
                    select new
                    {
                        Qty = line.QuantityLiters,
                        UnitRon = inv.Currency == "USD" ? line.UnitPrice * inv.FxToRon : line.UnitPrice
                    };
            var list = await q.ToListAsync(ct);
            if (list.Count == 0) return null;

            var totalQty = list.Sum(x => x.Qty);
            if (totalQty <= 0) return null;

            var totalCost = list.Sum(x => x.Qty * x.UnitRon);
            return totalCost / totalQty;
        }
    }
}
