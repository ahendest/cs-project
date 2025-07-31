using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class CorrectionLog : BaseEntity
    {
        public string TargetTable { get; set; } = string.Empty;
        public int TargetId { get; set; }
        public CorrectionType Type { get; set; }
        public string Reason { get; set; } = string.Empty;

        public int RequestedById { get; set; }
        public int? ApprovedById { get; set; }

        public DateTime RequestedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? ApprovedAtUtc { get; set; }
        public Guid AuditCorrelationId { get; set; }

        public EmployeeHistory? RequestedBy { get; set; }
        public EmployeeHistory? ApprovedBy { get; set; }
    }
}
