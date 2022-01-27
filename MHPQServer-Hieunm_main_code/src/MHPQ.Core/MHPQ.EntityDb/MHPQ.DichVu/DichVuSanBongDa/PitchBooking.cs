using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.EntityDb
{
    [Table("PitchBooking")]
    public class PitchBooking:AuditedEntity<long>
    {
        public long UserId { get; set; }
        //public long PitchId { get; set; }
        public long? FootballPitchId { set; get; }
        public FootballPitch FootballPitch { get; set; }
        public int TotalPrice { get; set; }
        public string IdentityNumber { get; set; }
        public string FullName { get; set; }
        public string TimeSlots { get; set; }
        public DateTime? ReserveDay { get; set; }
    }
}
