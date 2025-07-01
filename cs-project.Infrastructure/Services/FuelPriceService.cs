using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Repositories;

namespace cs_project.Infrastructure.Services
{
    public class FuelPriceService : IFuelPriceService
    {
        private readonly IFuelPriceRepository _fuelPriceRepository;
        private readonly IMapper _mapper;
        public FuelPriceService(IFuelPriceRepository fuelPriceRepository, IMapper mapper)
        {
            _fuelPriceRepository = fuelPriceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FuelPriceDTO>> GetAllAsync()
        {
            var fuelPrices = await _fuelPriceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<FuelPriceDTO>>(fuelPrices);
        }

        public async Task<FuelPriceDTO?> GetByIdAsync(int id)
        {
            var fuelPrice = await _fuelPriceRepository.GetByIdAsync(id);
            return fuelPrice == null ? null : _mapper.Map<FuelPriceDTO>(fuelPrice); 
        }

        public async Task<FuelPriceDTO> CreateAsync(FuelPriceCreateDTO fuelPrice)
        {
            var fuelPriceCreated = _mapper.Map<FuelPrice>(fuelPrice);
            await _fuelPriceRepository.AddAsync(fuelPriceCreated);
            await _fuelPriceRepository.SaveChangesAsync();
            return _mapper.Map<FuelPriceDTO>(fuelPriceCreated);
        }
        
        public async Task<bool> UpdateAsync(int id, FuelPriceCreateDTO fuelPrice)
        {
            var updatedPrice = await _fuelPriceRepository.GetByIdAsync(id);
            if (updatedPrice == null) return false;

            _mapper.Map(fuelPrice, updatedPrice);
            _fuelPriceRepository.Update(updatedPrice);
            return await _fuelPriceRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletePrice = await _fuelPriceRepository.GetByIdAsync(id);
            if (deletePrice == null) return false;
            _fuelPriceRepository.Delete(deletePrice);
            return await _fuelPriceRepository.SaveChangesAsync();
        }
    }   

}
