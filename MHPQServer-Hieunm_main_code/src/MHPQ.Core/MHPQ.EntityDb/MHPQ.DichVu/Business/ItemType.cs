using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace MHPQ.EntityDb
{
    public class ItemType: FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [StringLength(2000)]
        public string Name { get; set; }
        public bool? Required { get; set; }
        [StringLength(2000)]
        public string QueryKey { get; set; }
        public int? Type { get; set; }
        public int? InputType { get; set; }
        public string Value { get; set; }
        public string ListUnit { get; set; }
        public string Properties { get; set; }
    }
}
