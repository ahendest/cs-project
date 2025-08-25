using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities.Pricing;
using cs_project.Core.Interfaces;

namespace cs_project.Infrastructure.Services
{
    public class ExchangeRateService(IExchangeRateRepository repo, IMapper mapper) : IExchangeRateService
    {
        public async Task<IEnumerable<ExchangeRateDTO>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await repo.GetAllAsync(ct);
            return mapper.Map<IEnumerable<ExchangeRateDTO>>(entities);
        }

        public async Task<ExchangeRateDTO?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            return entity is null ? null : mapper.Map<ExchangeRateDTO>(entity);
        }

        public async Task<ExchangeRateDTO> CreateAsync(ExchangeRateCreateDTO dto, CancellationToken ct = default)
        {
            var entity = mapper.Map<ExchangeRate>(dto);
            await repo.AddAsync(entity, ct);
            await repo.SaveChangesAsync(ct);
            return mapper.Map<ExchangeRateDTO>(entity);
        }

        public async Task<bool> UpdateAsync(int id, ExchangeRateCreateDTO dto, CancellationToken ct = default)
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

