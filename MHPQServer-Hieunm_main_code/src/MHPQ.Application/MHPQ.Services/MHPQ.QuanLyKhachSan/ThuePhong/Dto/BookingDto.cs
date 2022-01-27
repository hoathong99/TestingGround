using Abp.AutoMapper;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan.ThuePhong
{
    [AutoMap(typeof(BookingRoomHotel))]
    public class BookingRoomHotelDto
    {
        public long? Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberPeople { get; set; }
        public long RoomHotelId { get; set; }
        public long GuestHotelId { get; set; }
        public bool IsPaid { get; set; }
        public int Money { get; set; }
    }
}
