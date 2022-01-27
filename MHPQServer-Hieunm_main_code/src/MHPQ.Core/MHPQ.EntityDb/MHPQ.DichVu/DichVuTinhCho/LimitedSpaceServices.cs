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
    [Table("LimitedSpaceServices")]
    public class LimitedSpaceServices : AuditedEntity<long>
    {
        [StringLength(256)]
        public string ServiceName { get; set; }
        public int ServiceType { get; set; }
        [StringLength(256)]
        public string PriceHours { get; set; }
        [StringLength(256)]
        public string TimeTable { get; set; }
        [StringLength(256)]
        public string Infor { get; set; }
        public int TotalSpace { get; set; }
        public int EmptySpace { get; set; }
    }
}
