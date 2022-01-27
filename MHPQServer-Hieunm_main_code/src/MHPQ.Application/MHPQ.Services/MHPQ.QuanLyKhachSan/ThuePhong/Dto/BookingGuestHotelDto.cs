using Abp.AutoMapper;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan.ThuePhong
{
    public class BookingGuestHotelDto
    {
        public long? Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberPeople { get; set; }
        public string RoomHotelName { get; set; }
        public string GuestHotelName { get; set; }
        public string GuestHotelIdentity { get; set; }
        public bool IsPaid { get; set; }
        public int Money { get; set; }
    }
}
