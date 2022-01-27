using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("Themes")]
    public class Theme: FullAuditedEntity<long>, IPassivable, IMayHaveTenant
    {
        [StringLength(256)]
        public string Name { get; set; }

        public long? RoomSmartHomeId { get; set; }

        public long? SmarthomeId { get; set; }

        public long? HomeServerId { get; set; }

        public bool IsActive { get; set; }

        public string ImageUrl { get; set; }

        public int? NumberDevices { get; set; }

        public string Value { get; set; }
        public int? TenantId { get; set; }
    }
}
