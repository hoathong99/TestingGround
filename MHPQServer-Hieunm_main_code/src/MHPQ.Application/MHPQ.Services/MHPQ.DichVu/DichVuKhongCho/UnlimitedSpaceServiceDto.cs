using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu
{
    public class UnlimitedSpaceServiceDto
    {
        public long UnlimitedSpaceServiceId { get; set; }
        public string ServiceName { get; set; }
        public int ServiceType { get; set; }
        public string PriceHours { get; set; }
        public string TimeTable { get; set; }
        public string Infor { get; set; }
    }
}
