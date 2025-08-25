using cs_project.Core.Entities;
using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Services
{
    public class SalesService : ISalesService
    {
        private readonly AppDbContext _db;
        private readonly IStationFuelPriceRepository _priceRepo;

        public SalesService(AppDbContext db, IStationFuelPriceRepository priceRepo)
        {
            _db = db;
            _priceRepo = priceRepo;
        }

        private async Task<(decimal unit, decimal total, DateTime now, int priceId)> GetPumpInfoAndTotalsAsync(int pumpId, decimal liters, CancellationToken ct)
        {
            var pump = await _db.Pumps.Include(p => p.Tank).ThenInclude(t => t.Station)
                                      .FirstOrDefaultAsync(p => p.Id == pumpId, ct)
                        ?? throw new InvalidOperationException("Pump not found");

            var stationId = pump.Tank.StationId;
            var fuelType = pump.Tank.FuelType;
            var now = DateTime.UtcNow;

            var price = await _priceRepo.GetCurrentAsync(stationId, fuelType, now, ct)
                        ?? throw new InvalidOperationException("No published price for station/fuel");

            var unit = price.PriceRon;
            var total = Math.Round(unit * liters, 2, MidpointRounding.AwayFromZero);

            return (unit, total, now, price.Id);
        }

        public async Task<CustomerTransaction> CreateSaleAsync(int pumpId, decimal liters, CancellationToken ct)
        {
            if (liters <= 0) throw new ArgumentOutOfRangeException(nameof(liters));

            var (unit, total, now, priceId) = await GetPumpInfoAndTotalsAsync(pumpId, liters, ct);

            var trx = new CustomerTransaction
            {
                PumpId = pumpId,
                Liters = liters,
                PricePerLiter = unit,
                TotalPrice = total,
                TimestampUtc = now,
                StationFuelPriceId = priceId
            };

            _db.CustomerTransactions.Add(trx);
            await _db.SaveChangesAsync(ct);
            return trx;
        }

        public async Task<IEnumerable<CustomerTransaction>> GetSalesAsync(CancellationToken ct)
            => await _db.CustomerTransactions.AsNoTracking().ToListAsync(ct);

        public async Task<CustomerTransaction?> GetSaleByIdAsync(int id, CancellationToken ct)
            => await _db.CustomerTransactions.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);

        public async Task<bool> UpdateSaleAsync(int id, int pumpId, decimal liters, CancellationToken ct)
        {
            if (liters <= 0) throw new ArgumentOutOfRangeException(nameof(liters));

            var trx = await _db.CustomerTransactions.FirstOrDefaultAsync(t => t.Id == id, ct);
            if (trx == null) return false;

            var (unit, total, now, priceId) = await GetPumpInfoAndTotalsAsync(pumpId, liters, ct);

            trx.PumpId = pumpId;
            trx.Liters = liters;
            trx.PricePerLiter = unit;
            trx.TotalPrice = total;
            trx.TimestampUtc = now;
            trx.StationFuelPriceId = priceId;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteSaleAsync(int id, CancellationToken ct)
        {
            var trx = await _db.CustomerTransactions.FirstOrDefaultAsync(t => t.Id == id, ct);
            if (trx == null) return false;

            _db.CustomerTransactions.Remove(trx);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}

