using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{

    [Table("DeviceAPIs")]
    public class DeviceApi: Entity<long>
    {
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Url { get; set; }
        [StringLength(2000)]
        public string Method { get; set; }
        [StringLength(2000)]
        public string DeviceName { get; set; }
        [StringLength(2000)]
        public string HomeServerName { get; set; }

        public int? Gateway { get; set; }

        public long? DeviceId { get; set; }

        public int? Port { get; set; }

        public long? HomeServerId { get; set; }

        public string Values { get; set; }
        [StringLength(2000)]
        public string ContentType { get; set; }
    }
}
