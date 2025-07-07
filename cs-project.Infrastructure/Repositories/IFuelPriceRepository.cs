using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface IFuelPriceRepository
    {
        Task<(IEnumerable<FuelPrice> Items, int TotalCount)> QueryFuelPricesAsync(PagingQueryParameters query);
        Task<IEnumerable<FuelPrice>> GetAllAsync();
        Task<FuelPrice?> GetByIdAsync(int id);
        Task AddAsync(FuelPrice fuelPrice);
        void Update(FuelPrice fuelPrice);
        void Delete(FuelPrice fuelPrice);
        Task<bool> SaveChangesAsync();

    }
}
