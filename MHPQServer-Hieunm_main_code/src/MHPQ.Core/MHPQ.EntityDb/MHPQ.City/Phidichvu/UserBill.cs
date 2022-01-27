

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{

    [Table("UserBills")]
    public class UserBill : FullAuditedEntity<long>, IMayHaveTenant
    {
        [StringLength(2000)]
        public string Name { get; set; }
        public string Properties { get; set; }
        public int? Type { get; set; }
        public int? TenantId { get; set; }
        public string QueryKey { get; set; }
    }
}