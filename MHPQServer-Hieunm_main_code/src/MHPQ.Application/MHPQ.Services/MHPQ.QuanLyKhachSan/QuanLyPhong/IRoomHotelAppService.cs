using Abp.Application.Services;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan.QuanLyPhong
{
    public interface IRoomHotelAppService : IApplicationService
    {
        Task<object> GetRooms();
        Task<object> GetRoomsByHotelId(long hotelId);
        Task<object> CreateRoom(RoomHotelDto roomDto);
        Task<object> UpdateRoom(RoomHotelDto roomDto);
        Task<object> DeleteRoom(RoomHotelDto roomDto);

        
    }
}
