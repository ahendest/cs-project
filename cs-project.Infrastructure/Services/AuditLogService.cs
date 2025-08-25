using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities.Audit;
using cs_project.Core.Models;
using cs_project.Infrastructure.Repositories;

namespace cs_project.Infrastructure.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IMapper _mapper;

        public AuditLogService(IAuditLogRepository auditLogRepository, IMapper mapper)
        {
            _auditLogRepository = auditLogRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<AuditLogDTO>> GetAuditLogAsync(PagingQueryParameters query)
        {
            var (entities, total) = await _auditLogRepository.QueryAuditLogsAsync(query);
            var dtoList = _mapper.Map<IEnumerable<AuditLogDTO>>(entities);

            return new PagedResult<AuditLogDTO>
            {
                Items = dtoList,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<IEnumerable<AuditLogDTO>> GetAllAuditLogsAsync()
        {
            var logs = await _auditLogRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AuditLogDTO>>(logs);
        }

        public async Task<AuditLogDTO?> GetAuditLogByIdAsync(long id)
        {
            var log = await _auditLogRepository.GetByIdAsync(id);
            return log == null ? null : _mapper.Map<AuditLogDTO>(log);
        }

        public async Task<AuditLogDTO> CreateAuditLogAsync(AuditLogCreateDTO dto)
        {
            var log = _mapper.Map<AuditLog>(dto);
            await _auditLogRepository.AddAsync(log);
            await _auditLogRepository.SaveChangesAsync();
            return _mapper.Map<AuditLogDTO>(log);
        }

        public async Task<bool> UpdateAuditLogAsync(long id, AuditLogCreateDTO dto)
        {
            var log = await _auditLogRepository.GetByIdAsync(id);
            if (log == null) return false;

            _mapper.Map(dto, log);
            _auditLogRepository.Update(log);
            return await _auditLogRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAuditLogAsync(long id)
        {
            var log = await _auditLogRepository.GetByIdAsync(id);
            if (log == null) return false;

            _auditLogRepository.Delete(log);
            return await _auditLogRepository.SaveChangesAsync();
        }
    }
}

