using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("SmartHomes")]
    public class SmartHome: FullAuditedEntity<long>, IMayHaveTenant
    {
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string UserName { get; set; }
        [StringLength(2000)]
        public string Address { get; set; }
        [StringLength(2000)]
        public string SmartHomeCode { get; set; }
        [StringLength(2000)]
        public string ImageUrl { get; set; }
        public int? Status { get; set; }
        public string PropertiesHistory { get; set; }
        public string Properties { get; set; }
        public int? TenantId { get; set; }
    }
}
