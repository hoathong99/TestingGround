using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("SensorDevices")]
    public class SensorDevice : FullAuditedEntity<long>, IDevice
    {
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Url { get; set; }

        public int? Port { get; set; }

        public long? HomeServerId { get; set; }

        public long? SmartHomeId { get; set; }

        public long? RoomId { get; set; }

        public long? FloorId { get; set; }
        [StringLength(2000)]
        public string HomeDeviceId { get; set; }
        [StringLength(256)]
        public string DeviceCode { get; set; }
        [StringLength(256)]
        public string TypeDevice { get; set; }
        [StringLength(2000)]
        public string HomeserverAvailables { get; set; }
        [StringLength(2000)]
        public string Producer { get; set; }

        public string Parameters { get; set; }
    }
}

