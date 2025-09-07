using cs_project.Core.Entities.Audit;
using cs_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace cs_project.Infrastructure.Data.Auditing
{
    public class AuditInterceptor(ICurrentUserAccessor userAccessor) : SaveChangesInterceptor
    {
        public readonly ICurrentUserAccessor _userAccessor = userAccessor;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private bool _isSaving;
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (_isSaving || eventData.Context is null) return result;

            try
            {
                _isSaving = true;
                var userId = _userAccessor.GetCurrentUserId() ?? string.Empty;
                var correlationId = Guid.NewGuid();
                var logs = new List<AuditLog>();

                foreach (var entry in eventData.Context.ChangeTracker.Entries())
                {
                    if (!ShouldAudit(entry)) continue;

                    logs.Add(CreateAuditLog(entry, userId, correlationId));
                }

                if (logs.Count > 0)
                {
                    foreach (var log in logs)
                        await AuditWriterService.Queue.Writer.WriteAsync(log, cancellationToken);
                }
            }
            finally
            {
                _isSaving = false;
            }

            return result;
        }

        private static bool ShouldAudit(EntityEntry entry) =>
            entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted &&
            entry.Entity is not AuditLog;

        private static AuditLog CreateAuditLog(EntityEntry entry, string userId, Guid correlationId)
        {
            var audit = new AuditLog
            {
                TableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name,
                RecordId = (long)entry.Properties.First(p => p.Metadata.IsPrimaryKey()).CurrentValue!,
                ModifiedBy = userId,
                ModifiedAt = DateTimeOffset.UtcNow,
                CorrelationId = correlationId
            };

            if (entry.State == EntityState.Added)
            {
                audit.Operation = "CREATE";
                audit.NewValues = Serialize(entry.CurrentValues.ToObject());
            }
            else if (entry.State == EntityState.Deleted)
            {
                audit.Operation = "DELETE";
                audit.OldValues = Serialize(entry.OriginalValues.ToObject());
            }
            else if (entry.State == EntityState.Modified)
            {
                audit.Operation = "UPDATE";

                var changes = entry.Properties
                    .Where(p => p.IsModified && !Equals(p.OriginalValue, p.CurrentValue))
                    .ToDictionary(
                        p => p.Metadata.Name,
                        p => new
                        {
                            Old = p.OriginalValue,
                            New = p.CurrentValue
                        });

                audit.OldValues = Serialize(changes.ToDictionary(x => x.Key, x => x.Value.Old));
                audit.NewValues = Serialize(changes.ToDictionary(x => x.Key, x => x.Value.New));
            }

            return audit;
        }

        private static string Serialize(object obj) //cencoring sensitive fields with [DoNotAudit] 
        {
            var type = obj.GetType();
            var dict = type.GetProperties()
                .Where(p => p.CanRead)
                .ToDictionary(
                    p => p.Name,
                    p =>
                        Attribute.IsDefined(p, typeof(DoNotAuditAttribute))
                            ? "**REDACTED**"
                            : p.GetValue(obj)
                );

            return JsonSerializer.Serialize(dict, _jsonOptions);
        }

    }
}
