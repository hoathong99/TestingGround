using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.EntityDb
{
    [Table("BookingRoomHotel")]
    public class BookingRoomHotel : AuditedEntity<long>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberPeople { get; set; }
        public long? RoomHotelId { get; set; }
        public RoomHotel RoomHotel { get; set; }
        public long? GuestHotelId { get; set; }
        public GuestHotel GuestHotel { get; set; }
        public bool IsPaid { get; set; }
        public int Money { get; set; }
    }
}
