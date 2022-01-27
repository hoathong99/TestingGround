using Abp.AutoMapper;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan.QuanLyPhong
{
    [AutoMap(typeof(RoomHotel))]
    public class RoomHotelDto
    {
        public long? Id { get; set; }
        public int Type { get; set; }
        public float Area { get; set; }
        public int Floor { get; set; }
        public bool IsRent { get; set; }
        public long? HotelId { get; set; }
        public int Price { get; set; }
    }
}
