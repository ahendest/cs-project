using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface IAuditLogService
    {
        Task<PagedResult<AuditLogDTO>> GetAuditLogAsync(PagingQueryParameters query);
        Task<IEnumerable<AuditLogDTO>> GetAllAuditLogsAsync();
        Task<AuditLogDTO?> GetAuditLogByIdAsync(long id);
        Task<AuditLogDTO> CreateAuditLogAsync(AuditLogCreateDTO dto);
        Task<bool> UpdateAuditLogAsync(long id, AuditLogCreateDTO dto);
        Task<bool> DeleteAuditLogAsync(long id);
    }
}
