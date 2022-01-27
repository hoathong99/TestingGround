using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("DeviceSomfies")]
    public class DeviceSomfy: FullAuditedEntity<long>
    {
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Type { get; set; }
        [StringLength(2000)]
        public string SiteId { get; set; }

        public long? HomeServerId { get; set; }

        public long? SmartHomeId { get; set; }

        public long? RoomId { get; set; }

        public long? FloorId { get; set; }
        [StringLength(2000)]
        public string HomeDeviceId { get; set; }
        [StringLength(2000)]
        public string Categories { get; set; }
        [StringLength(2000)]
        public string ParentId { get; set; }

        public string States { get; set; }
    }
}
