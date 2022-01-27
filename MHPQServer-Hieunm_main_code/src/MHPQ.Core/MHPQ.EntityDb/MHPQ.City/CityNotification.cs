
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("CityNotifications")]
    public class CityNotification : CreationAuditedEntity<long>, IMayHaveTenant, IDeletionAudited
    {
        [StringLength(2000)]
        public string Name { get; set; }
        public string Data { get; set; }
        [StringLength(2000)]
        public string FileUrl { get; set; }
        public int? Type { get; set; }
        public long? UserId { get; set; }
        public int? TenantId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get ; set ; }
        public bool IsDeleted { get; set; }
        public int? Follow { get; set; }
    }
}

