using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("BillViewSettings")]
    public class BillViewSetting : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? Type { get; set; }
        [StringLength(2000)]
        public string QueryKey { get; set; }
        public string Properties { get; set; }
        public int? TenantId { get; set; }
    }
}
