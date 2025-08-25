using cs_project.Core.Entities;
using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Repositories;

namespace cs_project.Infrastructure.Services
{
    public class SalesService : ISalesService
    {
        private readonly ICustomerTransactionRepository _transactionRepo;
        private readonly IPumpRepository _pumpRepo;
        private readonly ITankRepository _tankRepo;
        private readonly IStationFuelPriceRepository _priceRepo;

        public SalesService(ICustomerTransactionRepository transactionRepo,
                            IPumpRepository pumpRepo,
                            ITankRepository tankRepo,
                            IStationFuelPriceRepository priceRepo)
        {
            _transactionRepo = transactionRepo;
            _pumpRepo = pumpRepo;
            _tankRepo = tankRepo;
            _priceRepo = priceRepo;
        }

        private async Task<(decimal unit, decimal total, DateTime now, int priceId)> GetPumpInfoAndTotalsAsync(int pumpId, decimal liters, CancellationToken ct)
        {
            var pump = await _pumpRepo.GetByIdAsync(pumpId)
                        ?? throw new InvalidOperationException("Pump not found");

            var tank = await _tankRepo.GetByIdAsync(pump.TankId)
                        ?? throw new InvalidOperationException("Tank not found");

            var stationId = tank.StationId;
            var fuelType = tank.FuelType;
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

            await _transactionRepo.AddAsync(trx);
            await _transactionRepo.SaveChangesAsync();
            return trx;
        }

        public async Task<IEnumerable<CustomerTransaction>> GetSalesAsync(CancellationToken ct)
            => await _transactionRepo.GetAllAsync();

        public async Task<CustomerTransaction?> GetSaleByIdAsync(int id, CancellationToken ct)
            => await _transactionRepo.GetByIdAsync(id);

        public async Task<bool> UpdateSaleAsync(int id, int pumpId, decimal liters, CancellationToken ct)
        {
            if (liters <= 0) throw new ArgumentOutOfRangeException(nameof(liters));

            var trx = await _transactionRepo.GetByIdAsync(id);
            if (trx == null) return false;

            var (unit, total, now, priceId) = await GetPumpInfoAndTotalsAsync(pumpId, liters, ct);

            trx.PumpId = pumpId;
            trx.Liters = liters;
            trx.PricePerLiter = unit;
            trx.TotalPrice = total;
            trx.TimestampUtc = now;
            trx.StationFuelPriceId = priceId;

            _transactionRepo.Update(trx);
            return await _transactionRepo.SaveChangesAsync();
        }

        public async Task<bool> DeleteSaleAsync(int id, CancellationToken ct)
        {
            var trx = await _transactionRepo.GetByIdAsync(id);
            if (trx == null) return false;

            _transactionRepo.Delete(trx);
            return await _transactionRepo.SaveChangesAsync();
        }
    }
}

