using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.EntityDb
{
    [Table("HiresService")]
    public class Hires : AuditedEntity<long>
    {
        public long GuestServiceId { get; set; }
        public long LimitedSpaceServiceId { get; set; }
        public int NumberSpace { get; set; }
        public long UnlimitedSpaceServiceId { get; set; }
        public int HireHours { get; set; }
        public bool IsPaid { get; set; }
        public int Money { get; set; }
    }
}
