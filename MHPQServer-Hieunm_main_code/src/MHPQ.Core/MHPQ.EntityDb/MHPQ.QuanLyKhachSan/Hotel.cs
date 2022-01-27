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
    [Table("Hotel")]
    public class Hotel : AuditedEntity<long>
    {
        [StringLength(256)]
        public string HotelName { get; set; }
        [StringLength(256)]
        public string Address { get; set; }
        public ICollection<RoomHotel> RoomHotels { get; set; }
        public ICollection<GuestHotel> GuestHotels { get; set; }
    }
}
