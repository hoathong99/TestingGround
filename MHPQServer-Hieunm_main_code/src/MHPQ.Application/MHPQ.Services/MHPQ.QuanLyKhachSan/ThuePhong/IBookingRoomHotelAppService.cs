using Abp.Application.Services;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan.ThuePhong
{
    public interface IBookingRoomHotelAppService : IApplicationService
    {
        Task<object> GetAll();
        Task<object> GetBookingByHotelId(long hotelId);
        Task<object> GetEmptyRoom();
        Task<object> CreateBooking(BookingRoomHotelDto bookingDto);
        void CheckBooking();
    }
}
