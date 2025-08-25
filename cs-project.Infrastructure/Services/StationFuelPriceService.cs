using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities.Pricing;
using cs_project.Core.Interfaces;

namespace cs_project.Infrastructure.Services
{
    public class StationFuelPriceService(IStationFuelPriceRepository repo, IMapper mapper) : IStationFuelPriceService
    {
        public async Task<IEnumerable<StationFuelPriceDTO>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await repo.GetAllAsync(ct);
            return mapper.Map<IEnumerable<StationFuelPriceDTO>>(entities);
        }

        public async Task<StationFuelPriceDTO?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            return entity is null ? null : mapper.Map<StationFuelPriceDTO>(entity);
        }

        public async Task<StationFuelPriceDTO> CreateAsync(StationFuelPriceCreateDTO dto, CancellationToken ct = default)
        {
            var entity = mapper.Map<StationFuelPrice>(dto);
            await repo.AddAsync(entity, ct);
            await repo.SaveChangesAsync(ct);
            return mapper.Map<StationFuelPriceDTO>(entity);
        }

        public async Task<bool> UpdateAsync(int id, StationFuelPriceCreateDTO dto, CancellationToken ct = default)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return false;
            mapper.Map(dto, entity);
            repo.Update(entity);
            return await repo.SaveChangesAsync(ct);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return false;
            repo.Delete(entity);
            return await repo.SaveChangesAsync(ct);
        }
    }
}

