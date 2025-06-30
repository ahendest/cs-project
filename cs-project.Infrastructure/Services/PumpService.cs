using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Repositories;

namespace cs_project.Infrastructure.Services
{
    public class PumpService : IPumpService
    {
        private readonly IPumpRepository _pumpRepository;
        private readonly IMapper _mapper;

        public PumpService(IPumpRepository pumpRepository, IMapper mapper)
        {
            _pumpRepository = pumpRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PumpDTO>> GetAllPumpsAsync()
        {
            var pumps = await _pumpRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PumpDTO>>(pumps);
        }

        public async Task<PumpDTO?> GetPumpByIdAsync(int id)
        {
            var pump = await _pumpRepository.GetByIdAsync(id);
            return pump == null ? null : _mapper.Map<PumpDTO>(pump);
        }

        public async Task<PumpDTO> CreatePumpAsync(PumpCreateDTO pumpDto)
        {
            var pump = _mapper.Map<Pump>(pumpDto);
            await _pumpRepository.AddAsync(pump);
            await _pumpRepository.SaveChangesAsync();
            return _mapper.Map<PumpDTO>(pump);
        }

        public async Task<bool> UpdatePumpAsync(int id, PumpCreateDTO pumpDto)
        {
            var pump = await _pumpRepository.GetByIdAsync(id);
            if (pump == null) return false;

            _mapper.Map(pumpDto, pump);
            _pumpRepository.Update(pump);
            return await _pumpRepository.SaveChangesAsync();
        }

        public async Task<bool> DeletePumpAsync(int id)
        {
            var pump = await _pumpRepository.GetByIdAsync(id);
            if (pump == null) return false;

            _pumpRepository.Delete(pump);
            return await _pumpRepository.SaveChangesAsync();
        }

    }
}
