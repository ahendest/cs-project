using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Repositories;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public class TankService : ITankService
    {
        private readonly ITankRepository _tankRepository;
        private readonly IMapper _mapper;

        public TankService(ITankRepository tankRepository, IMapper mapper)
        {
            _tankRepository = tankRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<TankDTO>> GetTankAsync(PagingQueryParameters query)
        {
            var (entities, total) = await _tankRepository.QueryTanksAsync(query);
            var dtoList = _mapper.Map<IEnumerable<TankDTO>>(entities);

            return new PagedResult<TankDTO>
            {
                Items = dtoList,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<IEnumerable<TankDTO>> GetAllTanksAsync()
        {
            var tanks = await _tankRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TankDTO>>(tanks);
        }

        public async Task<TankDTO?> GetTankByIdAsync(int id)
        {
            var tank = await _tankRepository.GetByIdAsync(id);
            return tank == null ? null : _mapper.Map<TankDTO>(tank);
        }

        public async Task<TankDTO> CreateTankAsync(TankCreateDTO dto)
        {
            var tank = _mapper.Map<Tank>(dto);
            await _tankRepository.AddAsync(tank);
            await _tankRepository.SaveChangesAsync();
            return _mapper.Map<TankDTO>(tank);
        }

        public async Task<bool> UpdateTankAsync(int id, TankCreateDTO dto)
        {
            var tank = await _tankRepository.GetByIdAsync(id);
            if (tank == null) return false;

            _mapper.Map(dto, tank);
            _tankRepository.Update(tank);
            return await _tankRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteTankAsync(int id)
        {
            var tank = await _tankRepository.GetByIdAsync(id);
            if (tank == null) return false;

            _tankRepository.Delete(tank);
            return await _tankRepository.SaveChangesAsync();
        }
    }
}
