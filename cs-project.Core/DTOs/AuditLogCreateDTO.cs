using System.ComponentModel.DataAnnotations;

namespace cs_project.Core.DTOs
{
    public class AuditLogCreateDTO
    {
        [Required] public string TableName { get; set; } = string.Empty;
        [Required] public long RecordId { get; set; }
        [Required] public string Operation { get; set; } = string.Empty;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        [Required] public string ModifiedBy { get; set; } = string.Empty;
        [Required] public DateTimeOffset ModifiedAt { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
