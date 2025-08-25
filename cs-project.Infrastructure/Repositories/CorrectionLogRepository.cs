using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class CorrectionLogRepository : ICorrectionLogRepository
    {
        private readonly AppDbContext _db;

        public CorrectionLogRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<CorrectionLog> Items, int TotalCount)> QueryCorrectionLogsAsync(PagingQueryParameters query)
        {
            var logs = _db.CorrectionLogs.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var term = query.SearchTerm.ToLower();
                logs = logs.Where(l =>
                    l.TargetTable.ToLower().Contains(term) ||
                    l.Reason.ToLower().Contains(term) ||
                    l.Type.ToString().ToLower().Contains(term));
            }

            logs = query.SortBy?.ToLower() switch
            {
                "targettable" => logs.OrderBy(l => l.TargetTable),
                "targettable_desc" => logs.OrderByDescending(l => l.TargetTable),
                "requestedatutc" => logs.OrderBy(l => l.RequestedAtUtc),
                "requestedatutc_desc" => logs.OrderByDescending(l => l.RequestedAtUtc),
                _ => logs.OrderByDescending(l => l.Id)
            };

            int totalCount = await logs.CountAsync();

            int page = query.Page > 0 ? query.Page : 1;
            int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
            logs = logs.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await logs.ToListAsync();
            return (items, totalCount);
        }

        public async Task<IEnumerable<CorrectionLog>> GetAllAsync() =>
            await _db.CorrectionLogs.AsNoTracking().ToListAsync();

        public async Task<CorrectionLog?> GetByIdAsync(int id) =>
            await _db.CorrectionLogs.FindAsync(id);

        public async Task AddAsync(CorrectionLog log) =>
            await _db.CorrectionLogs.AddAsync(log);

        public void Update(CorrectionLog log) =>
            _db.CorrectionLogs.Update(log);

        public void Delete(CorrectionLog log) =>
            _db.CorrectionLogs.Remove(log);

        public async Task<bool> SaveChangesAsync() =>
            await _db.SaveChangesAsync() > 0;
    }
}

