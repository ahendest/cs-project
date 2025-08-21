using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface ICorrectionLogRepository
    {
        Task<(IEnumerable<CorrectionLog> Items, int TotalCount)> QueryCorrectionLogsAsync(PagingQueryParameters query);
        Task<IEnumerable<CorrectionLog>> GetAllAsync();
        Task<CorrectionLog?> GetByIdAsync(int id);
        Task AddAsync(CorrectionLog log);
        void Update(CorrectionLog log);
        void Delete(CorrectionLog log);
        Task<bool> SaveChangesAsync();
    }
}
