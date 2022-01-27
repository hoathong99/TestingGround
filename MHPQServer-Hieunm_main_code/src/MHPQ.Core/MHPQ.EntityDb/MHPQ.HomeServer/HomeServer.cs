using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("HomeServers")]
    public class HomeServer: FullAuditedEntity<long>, IMayHaveTenant
    {
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string HomeServerCode { get; set; }
        [StringLength(2000)]
        public string Ip { get; set; }

        public string TokenAuth { get; set; }
        [StringLength(256)]
        public string Type { get; set; }
        [StringLength(256)]
        public string TypeAuth { get; set; }
        [StringLength(2000)]
        public string IpAddress { get; set; }

        public int? Port { get; set; }

        public int? TokenType { get; set; }
        [StringLength(2000)]
        public string ExpiresIn { get; set; }
        [StringLength(2000)]
        public string Scope { get; set; }
        [StringLength(2000)]
        public string Producer { get; set; }
        [StringLength(2000)]
        public string ImageUrl { get; set; }
        [StringLength(2000)]
        public string UserLogin { get; set; }
        [StringLength(2000)]
        public string Password { get; set; }

        public string RefreshToken { get; set; }
        public int? TenantId { get; set; }
    }
}
