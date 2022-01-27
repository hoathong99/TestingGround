
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("RoomSmartHomes")]
    public class RoomSmartHome : FullAuditedEntity<long>, IMayHaveTenant
    {
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Number { get; set; }
        [StringLength(2000)]
        public string ImageUrl { get; set; }
        public long? SmartHomeId { get; set; }

        public long? FloorSmartHomeId { get; set; }

        public int? NumberDevices { get; set; }

        public bool? IsSmartLighting { get; set; }
        public bool? IsSmartCurtain { get; set; }
        public bool? IsSmartAir { get; set; }
        public bool? IsSmartWatter { get; set; }
        public bool? IsSmartDoorEntry { get; set; }
        public bool? IsSmartConnection { get; set; }
        public bool? IsSmartConditioner { get; set; }
        public bool? IsSmartFireAlarm { get; set; }
        public bool? IsSmartSecurity { get; set; }
        public int? TenantId { get; set; }
    }

}
