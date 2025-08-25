using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities.Pricing;
using cs_project.Core.Interfaces;

namespace cs_project.Infrastructure.Services
{
    public class PricePolicyService(IPricePolicyRepository repo, IMapper mapper) : IPricePolicyService
    {
        public async Task<IEnumerable<PricePolicyDTO>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await repo.GetAllAsync(ct);
            return mapper.Map<IEnumerable<PricePolicyDTO>>(entities);
        }

        public async Task<PricePolicyDTO?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            return entity is null ? null : mapper.Map<PricePolicyDTO>(entity);
        }

        public async Task<PricePolicyDTO> CreateAsync(PricePolicyCreateDTO dto, CancellationToken ct = default)
        {
            var entity = mapper.Map<PricePolicy>(dto);
            await repo.AddAsync(entity, ct);
            await repo.SaveChangesAsync(ct);
            return mapper.Map<PricePolicyDTO>(entity);
        }

        public async Task<bool> UpdateAsync(int id, PricePolicyCreateDTO dto, CancellationToken ct = default)
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

