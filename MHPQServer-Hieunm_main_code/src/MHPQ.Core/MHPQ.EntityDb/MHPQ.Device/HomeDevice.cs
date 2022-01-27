using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.EntityDb
{
    [Table("HomeDevices")]
    public class HomeDevice : FullAuditedEntity<long>, IMayHaveTenant
    {
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Url { get; set; }

        public int? Port { get; set; }

        public long? HomeServerId { get; set; }

        public long? HomeGatewayId { get; set; }
        public long DeviceSettingId { get; set; }

        public int? DeviceGateway { get; set; }

        public long? SmartHomeId { get; set; }

        public long? RoomId { get; set; }

        public long? FloorId { get; set; }
        [StringLength(2000)]
        public string HomeDeviceId { get; set; }
        [StringLength(2000)]
        public string HomeDeviceAddress { get; set; }
        [StringLength(256)]
        public string DeviceCode { get; set; }
        [StringLength(2000)]
        public string TypeDevice { get; set; }
        [StringLength(2000)]
        public string GroupDevice { get; set; }
        [StringLength(2000)]
        public string Producer { get; set; }

        public string Parameters { get; set; }
        [StringLength(2000)]
        public string ImageUrl { get; set; }

        public int? DeviceNumber { get; set; }
        public int? TenantId { get ; set; }
    }
}
