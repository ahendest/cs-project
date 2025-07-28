using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_project.Core.Entities.Audit
{
    public class AuditLog
    {
        public long Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public long RecordId { get; set; }
        public string Operation { get; set; } = string.Empty;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public long ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
