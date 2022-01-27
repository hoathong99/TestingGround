using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu.Dto
{
    public class BookingGuestPitchDto
    {
        public long? GuestPitchId { get; set; }
        public string GuestPitchName { get; set; }
        public string IdentityNumber { get; set; }
        public long? FootballPitchId { set; get; }
        public string FootballPitchName { get; set; }
        public int TotalPrice { get; set; }
        public string TimeSlots { get; set; }
        public DateTime? ReserveDay { get; set; }
    }
}
