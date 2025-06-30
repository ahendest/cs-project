using cs_project.Core.DTOs;

namespace cs_project.Infrastructure.Services
{
    public interface IFuelPriceService
    {
        Task<IEnumerable<FuelPriceDTO>> GetAllAsync();
        Task<FuelPriceDTO?> GetByIdAsync(int id); 
        Task<FuelPriceDTO> CreateAsync(FuelPriceCreateDTO fuelPrice);
        Task<bool> UpdateAsync(int id, FuelPriceCreateDTO fuelPrice);
        Task<bool> DeleteAsync(int id);
    }
}
