using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;


namespace MHPQ.EntityDb
{
    public class UserFeedbackComment: FullAuditedEntity<long>, IMayHaveTenant
    {
        public long FeedbackId { get; set; }
        public string Comment { get; set; }
        public int? TenantId { get; set; }
        public string FileUrl { get; set; }
    }
}
