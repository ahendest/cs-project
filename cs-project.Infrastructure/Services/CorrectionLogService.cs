using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Repositories;

namespace cs_project.Infrastructure.Services
{
    public class CorrectionLogService : ICorrectionLogService
    {
        private readonly ICorrectionLogRepository _correctionLogRepository;
        private readonly IMapper _mapper;

        public CorrectionLogService(ICorrectionLogRepository correctionLogRepository, IMapper mapper)
        {
            _correctionLogRepository = correctionLogRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<CorrectionLogDTO>> GetCorrectionLogAsync(PagingQueryParameters query)
        {
            var (entities, total) = await _correctionLogRepository.QueryCorrectionLogsAsync(query);
            var dtoList = _mapper.Map<IEnumerable<CorrectionLogDTO>>(entities);

            return new PagedResult<CorrectionLogDTO>
            {
                Items = dtoList,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<IEnumerable<CorrectionLogDTO>> GetAllCorrectionLogsAsync()
        {
            var logs = await _correctionLogRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CorrectionLogDTO>>(logs);
        }

        public async Task<CorrectionLogDTO?> GetCorrectionLogByIdAsync(int id)
        {
            var log = await _correctionLogRepository.GetByIdAsync(id);
            return log == null ? null : _mapper.Map<CorrectionLogDTO>(log);
        }

        public async Task<CorrectionLogDTO> CreateCorrectionLogAsync(CorrectionLogCreateDTO dto)
        {
            var log = _mapper.Map<CorrectionLog>(dto);
            await _correctionLogRepository.AddAsync(log);
            await _correctionLogRepository.SaveChangesAsync();
            return _mapper.Map<CorrectionLogDTO>(log);
        }

        public async Task<bool> UpdateCorrectionLogAsync(int id, CorrectionLogCreateDTO dto)
        {
            var log = await _correctionLogRepository.GetByIdAsync(id);
            if (log == null) return false;

            _mapper.Map(dto, log);
            _correctionLogRepository.Update(log);
            return await _correctionLogRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteCorrectionLogAsync(int id)
        {
            var log = await _correctionLogRepository.GetByIdAsync(id);
            if (log == null) return false;

            _correctionLogRepository.Delete(log);
            return await _correctionLogRepository.SaveChangesAsync();
        }
    }
}

