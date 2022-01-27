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
    [Table("FootballPitch")]
    public class FootballPitch : Entity<long>
    {
        [StringLength(256)]
        public string Name { get; set; }
        public bool State { get; set; }
        public int Price { get; set; }
        public ICollection<PitchBooking> PitchBookings { get; set; }
    }
}
