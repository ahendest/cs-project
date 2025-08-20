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
        
        public async Task<CustomerTransaction> CreateSaleAsync(int pumpId, decimal liters, CancellationToken ct)
        {
            if (liters <= 0) throw new ArgumentOutOfRangeException(nameof(liters));

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

            var trx = new CustomerTransaction
            {
                PumpId = pumpId,
                Liters = liters,
                PricePerLiter = unit,
                TotalPrice = total,
                TimestampUtc = now,
                StationFuelPriceId = price.Id
            };

            _db.CustomerTransactions.Add(trx);
            await _db.SaveChangesAsync(ct);
            return trx;
        }
        
    }
}
