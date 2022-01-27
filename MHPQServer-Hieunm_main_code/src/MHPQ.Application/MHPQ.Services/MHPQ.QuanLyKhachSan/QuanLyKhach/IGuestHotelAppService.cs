using Abp.Application.Services;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan.QuanLyKhach
{
    public interface IGuestHotelAppService : IApplicationService
    {
        Task<object> GetAll();
        Task<object> GetGuestHotelsByHotelId(long hotelId);
        Task<object> Create(GuestHotelDto guestDto);
        Task<object> Update(GuestHotelDto guestDto);
        Task<object> Delete(GuestHotelDto guestDto);

        
    }
}
