using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("HomeGateways")]
    public class HomeGateway : FullAuditedEntity<long>, IMayHaveTenant
    {
        [StringLength(256)]
        public string Name { get; set; }
        public string TokenAuth { get; set; }
        [StringLength(256)]
        public string Type { get; set; }
        [StringLength(256)]
        public string TypeAuth { get; set; }
        [StringLength(2000)]
        public string IpAddress { get; set; }

        public long? HomeServerId { get; set; }

        public int? Port { get; set; }

        public int? TokenType { get; set; }
        [StringLength(2000)]
        public string UserLogin { get; set; }
        [StringLength(2000)]
        public string Password { get; set; }
        [StringLength(2000)]
        public string RefreshToken { get; set; }

        public long? SmartHomeId { get; set; }

        [StringLength(2000)]
        public string ImageUrl { get; set; }
        public int? TenantId { get; set; }
    }
}
