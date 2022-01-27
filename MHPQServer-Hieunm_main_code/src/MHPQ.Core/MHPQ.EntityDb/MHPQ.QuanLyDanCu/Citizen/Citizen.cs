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
    [Table("Citizen")]
    public class Citizen : FullAuditedEntity<long>, IMayHaveTenant
    {
        [StringLength(256)]
        public string FullName { get; set; }
        [StringLength(1000)]
        public string HomeAddress { get; set; }
        [StringLength(1000)]
        public string Address { get; set; }
        [StringLength(256)]
        public string Nationality { get; set; }
        [StringLength(256)]
        public string IdentityNumber { get; set; }
        public int? TenantId { get ; set; }
        public long? AccountId { get; set; }
        public string ImageUrl { get; set; }
        [StringLength(256)]
        public string PhoneNumber { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
