using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;


namespace MHPQ.EntityDb
{
    public class SetItems: FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        public string Properties { get; set; }
        public string Items { get; set; }
    }
}
