using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace MHPQ.EntityDb
{
    public class Order: FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Items { get; set; }
        public long? ObjectPartnerId { get; set; }
        public long? OrdererId { get; set; }
        public string Orderer { get; set; }
        public string Properties { get; set; }
        public int? Type { get; set; }
        public int? State { get; set; }
    }
}
