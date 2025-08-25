using cs_project.Core.Entities.Audit;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _db;

        public AuditLogRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<AuditLog> Items, int TotalCount)> QueryAuditLogsAsync(PagingQueryParameters query)
        {
            var logs = _db.AuditLogs.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var term = query.SearchTerm.ToLower();
                logs = logs.Where(l =>
                    l.TableName.ToLower().Contains(term) ||
                    l.Operation.ToLower().Contains(term) ||
                    l.CorrelationId.ToString().ToLower().Contains(term));
            }

            logs = query.SortBy?.ToLower() switch
            {
                "tablename" => logs.OrderBy(l => l.TableName),
                "tablename_desc" => logs.OrderByDescending(l => l.TableName),
                "modifiedat" => logs.OrderBy(l => l.ModifiedAt),
                "modifiedat_desc" => logs.OrderByDescending(l => l.ModifiedAt),
                _ => logs.OrderByDescending(l => l.Id)
            };

            int totalCount = await logs.CountAsync();

            int page = query.Page > 0 ? query.Page : 1;
            int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            logs = logs.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await logs.ToListAsync();
            return (items, totalCount);
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync() =>
            await _db.AuditLogs.AsNoTracking().ToListAsync();

        public async Task<AuditLog?> GetByIdAsync(long id) =>
            await _db.AuditLogs.FindAsync(id);

        public async Task AddAsync(AuditLog log) =>
            await _db.AuditLogs.AddAsync(log);

        public void Update(AuditLog log) =>
            _db.AuditLogs.Update(log);

        public void Delete(AuditLog log) =>
            _db.AuditLogs.Remove(log);

        public async Task<bool> SaveChangesAsync() =>
            await _db.SaveChangesAsync() > 0;
    }
}

