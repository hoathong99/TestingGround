using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("SwitchDevices")]
    public class SwitchDevice : FullAuditedEntity<long>, IDevice
    {
        [StringLength(256)]
        public string Name { get; set; }

        public int? Type { get; set; }
        [StringLength(2000)]
        public string DeviceIp { get; set; }
        [StringLength(2000)]
        public string Url { get; set; }

        public int? Port { get; set; }

        public long? HomeServerId { get; set; }

        public long? SmartHomeId { get; set; }

        public long? RoomId { get; set; }

        public long? FloorId { get; set; }
        [StringLength(2000)]
        public string HomeDeviceId { get; set; }
    }
}
