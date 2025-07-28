
namespace cs_project.Infrastructure.Data.Auditing
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DoNotAuditAttribute : Attribute { }
}
