using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan.QuanLyPhong
{
    public class RoomHotelAppService : MHPQAppServiceBase, IRoomHotelAppService
    {
        private readonly IRepository<RoomHotel, long> _roomHotelRepo;

        public RoomHotelAppService(IRepository<RoomHotel, long> roomHotelRepo)
        {
            _roomHotelRepo = roomHotelRepo;
        }

        public async Task<object> GetRooms()
        {
            try
            {
                var result = await _roomHotelRepo.GetAllListAsync();
                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.GetAllSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        public async Task<object> GetRoomsByHotelId(long hotelId)
        {
            try
            {
                var result = from rooms in _roomHotelRepo.GetAll()
                             where rooms.HotelId == hotelId
                             select rooms;
                var data = DataResult.ResultSucces(result.ToList(), Common.Resource.QuanLyChung.GetAllSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        public async Task<object> CreateRoom(RoomHotelDto roomDto)
        {
            try
            {
                var roomEntity = ObjectMapper.Map<RoomHotel>(roomDto);
                await _roomHotelRepo.InsertAsync(roomEntity);
                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.InsertSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        public async Task<object> UpdateRoom(RoomHotelDto roomDto)
        {
            try
            {
                var roomEntity = ObjectMapper.Map<RoomHotel>(roomDto);
                await _roomHotelRepo.UpdateAsync(roomEntity);
                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.UpdateSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        public async Task<object> DeleteRoom(RoomHotelDto roomDto)
        {
            try
            {
                var roomEntity = ObjectMapper.Map<RoomHotel>(roomDto);
                await _roomHotelRepo.DeleteAsync(roomEntity);
                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.DeleteSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        
    }
}
