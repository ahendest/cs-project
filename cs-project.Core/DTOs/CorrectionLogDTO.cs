using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class CorrectionLogDTO
    {
        public int Id { get; set; }
        public string TargetTable { get; set; } = string.Empty;
        public int TargetId { get; set; }
        public CorrectionType Type { get; set; }
        public string Reason { get; set; } = string.Empty;
        public int RequestedById { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime RequestedAtUtc { get; set; }
        public DateTime? ApprovedAtUtc { get; set; }
        public Guid AuditCorrelationId { get; set; }
    }
}
