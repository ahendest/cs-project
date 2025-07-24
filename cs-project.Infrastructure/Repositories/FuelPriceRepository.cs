using cs_project.Core.Entities;
using cs_project.Core.Models;
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

        public async Task<(IEnumerable<FuelPrice> Items, int TotalCount)> QueryFuelPricesAsync(PagingQueryParameters query)
        {
            var prices = _fuelPriceService.FuelPrices.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                string term = query.SearchTerm.ToLower();
                prices = prices.Where(fp =>
                         fp.FuelType.ToString().ToLower().Contains(term));
            }
            prices = query.SortBy?.ToLower() switch
            {
                "fueltype_desc" => prices.OrderByDescending(fp => fp.FuelType),
                "updatedat" => prices.OrderBy(fp => fp.UpdatedAt),
                "updatedat_desc" => prices.OrderByDescending(fp => fp.UpdatedAt),
                _ => prices.OrderBy(fp => fp.FuelType)
            };
            int total = await prices.CountAsync();

            int page = query.Page > 0 ? query.Page : 1;
            int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            prices = prices.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await prices.ToListAsync();

            return (items, total);
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
