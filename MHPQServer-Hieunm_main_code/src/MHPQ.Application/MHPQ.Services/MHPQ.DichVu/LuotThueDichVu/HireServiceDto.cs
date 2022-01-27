using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu
{
    public class HireServiceDto
    {
        public long HireId { get; set; }
        public long GuestServiceId { get; set; }
        public long LimitedSpaceServiceId { get; set; }
        public int NumberSpace { get; set; }
        public long UnlimitedSpaceServiceId { get; set; }
        public int HireHours { get; set; }
    }
}
