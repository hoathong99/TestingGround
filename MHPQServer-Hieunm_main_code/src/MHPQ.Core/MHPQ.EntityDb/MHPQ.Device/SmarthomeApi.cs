using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{

    [Table("SmarthomeAPIs")]
    public class SmarthomeApi : Entity<long>
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

        public int? GateWay { get; set; }

        public long? DeviceId { get; set; }

        public int? Port { get; set; }

        public long? HomeServerId { get; set; }

        public string Values { get; set; }
        [StringLength(2000)]
        public string ContentType { get; set; }

        public long? SmarthomeId { get; set; }

        public long? DeviceSmarthomeId { get; set; }
    }
}
