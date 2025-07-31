namespace cs_project.Core.History
{
    public abstract class HistoryBase
    {
        public long AuditId { get; set; }
        public int AuditUserId { get; set; }
        public DateTime AuditTsUtc { get; set; }
        public char AuditOp { get; set; }
        public Guid AuditCorrelation { get; set; }
    }
}
