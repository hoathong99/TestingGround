using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.EntityDb
{
    [Table("UnlimitedSpaceServices")]
    public class UnlimitedSpaceServices : AuditedEntity<long>
    {
        public string ServiceName { get; set; }
        public int ServiceType { get; set; }
        public string PriceHours { get; set; }
        public string TimeTable { get; set; }
        public string Infor { get; set; }
    }
}
