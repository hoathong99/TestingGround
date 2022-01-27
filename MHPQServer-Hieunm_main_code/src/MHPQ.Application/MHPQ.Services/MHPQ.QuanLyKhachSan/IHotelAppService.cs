using Abp.Application.Services;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan
{
    public interface IHotelAppService : IApplicationService
    {
        Task<object> GetHotels();
        Task<object> CreateHotel(HotelDto hotelDto);
        Task<object> UpdateHotel(HotelDto hotelDto);
        Task<object> DeleteHotel(HotelDto hotelDto);

        
    }
}
