using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface ICorrectionLogService
    {
        Task<PagedResult<CorrectionLogDTO>> GetCorrectionLogAsync(PagingQueryParameters query);
        Task<IEnumerable<CorrectionLogDTO>> GetAllCorrectionLogsAsync();
        Task<CorrectionLogDTO?> GetCorrectionLogByIdAsync(int id);
        Task<CorrectionLogDTO> CreateCorrectionLogAsync(CorrectionLogCreateDTO dto);
        Task<bool> UpdateCorrectionLogAsync(int id, CorrectionLogCreateDTO dto);
        Task<bool> DeleteCorrectionLogAsync(int id);
    }
}
