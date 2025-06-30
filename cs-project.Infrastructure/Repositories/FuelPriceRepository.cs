using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class FuelPriceRepository : IFuelPriceRepository
    {
        private readonly AppDbContext _fuelPriceService;
        public FuelPriceRepository(AppDbContext fuelPriceService)
        {
            _fuelPriceService = fuelPriceService;
        }

        public async Task<IEnumerable<FuelPrice>> GetAllAsync() =>
            await _fuelPriceService.FuelPrices.ToListAsync();

        public async Task<FuelPrice?> GetByIdAsync(int id) =>
            await _fuelPriceService.FuelPrices.FindAsync(id);

        public async Task AddAsync(FuelPrice fuelPrice) =>
            await _fuelPriceService.FuelPrices.AddAsync(fuelPrice);

        public void Update(FuelPrice fuelPrice) =>
            _fuelPriceService.FuelPrices.Update(fuelPrice);

        public void Delete(FuelPrice fuelPrice) =>
            _fuelPriceService.FuelPrices.Remove(fuelPrice);

        public async Task<bool> SaveChangesAsync() =>
            await _fuelPriceService.SaveChangesAsync() > 0;
    }
}
