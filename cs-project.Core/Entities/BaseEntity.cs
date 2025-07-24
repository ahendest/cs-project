
namespace cs_project.Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; set; }
        public bool IsActive { get; set; } = true;
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();   // concurrency
        
    }
}
