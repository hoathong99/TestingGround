using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("Houses")]
    public class House: FullAuditedEntity<long>, IPassivable
    {
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Address { get; set; }
        [StringLength(2000)]
        public string SmartHomeCode { get; set; }

        public bool IsActive { get; set; }
    }
}
