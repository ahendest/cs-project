using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Repositories;

namespace cs_project.Infrastructure.Services
{
    public class StationService : IStationService
    {
        private readonly IStationRepository _stationRepository;
        private readonly IMapper _mapper;

        public StationService(IStationRepository stationRepository, IMapper mapper)
        {
            _stationRepository = stationRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<StationDTO>> GetStationAsync(PagingQueryParameters query, CancellationToken ct)
        {
            var (entities, total) = await _stationRepository.QueryStationsAsync(query);
            var dtoList = _mapper.Map<IEnumerable<StationDTO>>(entities);

            return new PagedResult<StationDTO>
            {
                Items = dtoList,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<IEnumerable<StationDTO>> GetAllStationsAsync(CancellationToken ct)
        {
            var stations = await _stationRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StationDTO>>(stations);
        }

        public async Task<StationDTO?> GetStationByIdAsync(int id, CancellationToken ct)
        {
            var station = await _stationRepository.GetByIdAsync(id);
            return station == null ? null : _mapper.Map<StationDTO>(station);
        }

        public async Task<StationDTO> CreateStationAsync(StationCreateDTO dto, CancellationToken ct)
        {
            var station = _mapper.Map<Station>(dto);
            await _stationRepository.AddAsync(station);
            await _stationRepository.SaveChangesAsync();
            return _mapper.Map<StationDTO>(station);
        }

        public async Task<bool> UpdateStationAsync(int id, StationCreateDTO dto, CancellationToken ct)
        {
            var station = await _stationRepository.GetByIdAsync(id);
            if (station == null) return false;

            _mapper.Map(dto, station);
            _stationRepository.Update(station);
            return await _stationRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteStationAsync(int id, CancellationToken ct)
        {
            var station = await _stationRepository.GetByIdAsync(id);
            if (station == null) return false;

            _stationRepository.Delete(station);
            return await _stationRepository.SaveChangesAsync();
        }
    }
}
