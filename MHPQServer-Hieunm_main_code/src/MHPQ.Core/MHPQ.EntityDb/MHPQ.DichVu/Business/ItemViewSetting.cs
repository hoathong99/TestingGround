using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace MHPQ.EntityDb
{
    public class ItemViewSetting : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public int? Type { get; set; }
        [StringLength(2000)]
        public string QueryKey { get; set; }
        public string Properties { get; set; }

    }
}
