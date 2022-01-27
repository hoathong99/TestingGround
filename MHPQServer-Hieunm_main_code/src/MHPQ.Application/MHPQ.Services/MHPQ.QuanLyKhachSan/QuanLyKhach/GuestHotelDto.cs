using Abp.AutoMapper;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan.QuanLyKhach
{
    [AutoMap(typeof(GuestHotel))]
    public class GuestHotelDto
    {
        public long? Id { get; set; }
        public long? HotelId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentityNumber { get; set; }
    }
}
