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
    [Table("HouseOwner")]
    public class HouseOwner : FullAuditedEntity<long>, IMayHaveTenant
    {
        public long SmartHomeId { get; set; }
        [StringLength(2000)]
        public string SmartHomeCode { get; set; }
        public long? UserId { get; set; }
        public bool? IsVote { get; set; }
        public bool? IsAdmin { get; set; }                                                                                                  
        public int? TenantId { get; set; }
    }
}
