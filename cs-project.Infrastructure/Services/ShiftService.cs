using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Repositories;

namespace cs_project.Infrastructure.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IMapper _mapper;

        public ShiftService(IShiftRepository shiftRepository, IMapper mapper)
        {
            _shiftRepository = shiftRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ShiftDTO>> GetShiftAsync(PagingQueryParameters query)
        {
            var (entities, total) = await _shiftRepository.QueryShiftsAsync(query);
            var dtoList = _mapper.Map<IEnumerable<ShiftDTO>>(entities);

            return new PagedResult<ShiftDTO>
            {
                Items = dtoList,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<IEnumerable<ShiftDTO>> GetAllShiftsAsync()
        {
            var shifts = await _shiftRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ShiftDTO>>(shifts);
        }

        public async Task<ShiftDTO?> GetShiftByIdAsync(int id)
        {
            var shift = await _shiftRepository.GetByIdAsync(id);
            return shift == null ? null : _mapper.Map<ShiftDTO>(shift);
        }

        public async Task<ShiftDTO> CreateShiftAsync(ShiftCreateDTO dto)
        {
            var shift = _mapper.Map<Shift>(dto);
            await _shiftRepository.AddAsync(shift);
            await _shiftRepository.SaveChangesAsync();
            return _mapper.Map<ShiftDTO>(shift);
        }

        public async Task<bool> UpdateShiftAsync(int id, ShiftCreateDTO dto)
        {
            var shift = await _shiftRepository.GetByIdAsync(id);
            if (shift == null) return false;

            _mapper.Map(dto, shift);
            _shiftRepository.Update(shift);
            return await _shiftRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteShiftAsync(int id)
        {
            var shift = await _shiftRepository.GetByIdAsync(id);
            if (shift == null) return false;

            _shiftRepository.Delete(shift);
            return await _shiftRepository.SaveChangesAsync();
        }
    }
}
