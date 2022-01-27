using Abp.AutoMapper;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu.Dto
{
    [AutoMapTo(typeof(PitchBooking))]
    public class PitchBookingDto
    {
        public long UserId { get; set; }
        //public long PitchId { get; set; }
        public long? FootballPitchId { set; get; }
        public int TotalPrice { get; set; }
        public string IdentityNumber { get; set; }
        public string FullName { get; set; }
        public string TimeSlots { get; set; }
        public DateTime? ReserveDay { get; set; }
    }

    //public class TimeSlotDto
    //{
    //    public long? Id { get; set; }
    //    public string? Time { get; set; }
    //    public bool? IsSelected { get; set; }
    //    public int? StartBooking { get; set; }
    //    public int? EndBooking { get; set; }
    //}

    //public class PitchBookingTimeSlotsDto
    //{
    //    public PitchBookingDto PitchBooking { get; set; }
    //    public List<TimeSlotDto> TimeSlots { get; set; }
    //}
}
