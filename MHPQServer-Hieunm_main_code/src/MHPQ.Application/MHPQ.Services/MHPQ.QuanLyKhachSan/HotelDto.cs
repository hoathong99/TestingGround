using Abp.AutoMapper;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan
{
    [AutoMap(typeof(Hotel))]
    public class HotelDto
    {
        public long? Id { get; set; }
        public string HotelName { get; set; }
        public string Address { get; set; }
    }
}
