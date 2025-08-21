using System.ComponentModel.DataAnnotations;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class CorrectionLogCreateDTO
    {
        [Required] public string TargetTable { get; set; } = string.Empty;
        [Range(1, int.MaxValue)] public int TargetId { get; set; }
        [Required] public CorrectionType Type { get; set; }
        [Required] public string Reason { get; set; } = string.Empty;
        [Required] public int RequestedById { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime RequestedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? ApprovedAtUtc { get; set; }
        public Guid AuditCorrelationId { get; set; }
    }
}
