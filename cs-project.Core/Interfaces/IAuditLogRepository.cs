using cs_project.Core.Entities.Audit;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface IAuditLogRepository
    {
        Task<(IEnumerable<AuditLog> Items, int TotalCount)> QueryAuditLogsAsync(PagingQueryParameters query);
        Task<IEnumerable<AuditLog>> GetAllAsync();
        Task<AuditLog?> GetByIdAsync(long id);
        Task AddAsync(AuditLog log);
        void Update(AuditLog log);
        void Delete(AuditLog log);
        Task<bool> SaveChangesAsync();
    }
}
