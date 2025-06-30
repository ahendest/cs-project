using cs_project.Core.Entities;

namespace cs_project.Infrastructure.Repositories
{
    public interface IFuelPriceRepository
    {
        Task<IEnumerable<FuelPrice>> GetAllAsync();
        Task<FuelPrice?> GetByIdAsync(int id);
        Task AddAsync(FuelPrice fuelPrice);
        void Update(FuelPrice fuelPrice);
        void Delete(FuelPrice fuelPrice);
        Task<bool> SaveChangesAsync();

    }
}
