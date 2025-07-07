using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface IFuelPriceService
    {
        Task<PagedResult<FuelPriceDTO>> GetFuelPricesAsync(PagingQueryParameters query);
        Task<IEnumerable<FuelPriceDTO>> GetAllAsync();
        Task<FuelPriceDTO?> GetByIdAsync(int id); 
        Task<FuelPriceDTO> CreateAsync(FuelPriceCreateDTO fuelPrice);
        Task<bool> UpdateAsync(int id, FuelPriceCreateDTO fuelPrice);
        Task<bool> DeleteAsync(int id);
    }
}
