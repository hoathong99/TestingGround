using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace MHPQ.EntityDb
{
    public class ObjectPartner : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public int? Type { get; set; }
        [StringLength(2000)]
        public string QueryKey { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string PropertyHistories { get; set; }
        public string Properties { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Owner { get; set; }
        [StringLength(2000)]
        public string Operator { get; set; }
        public int? Like { get; set; }
        public int? State { get; set; }
        public string StateProperties { get; set; }
    }
}
