using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan.QuanLyKhach
{
    public class GuestHotelAppService : MHPQAppServiceBase, IGuestHotelAppService
    {
        private readonly IRepository<GuestHotel, long> _guestHotelRepository;

        public GuestHotelAppService(IRepository<GuestHotel, long> guestHotelRepository)
        {
            _guestHotelRepository = guestHotelRepository;
        }

        public async Task<object> GetAll()
        {
            try
            {
                var result = await _guestHotelRepository.GetAllListAsync();

                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.GetAllSuccess);
                return data;
            }
            catch (Exception e)
            {               
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        public async Task<object> GetGuestHotelsByHotelId(long hotelId)
        {
            try
            {
                var result = from guestHotels in _guestHotelRepository.GetAll()
                             where guestHotels.HotelId == hotelId
                             select guestHotels;

                var data = DataResult.ResultSucces(result.ToList(), Common.Resource.QuanLyChung.GetAllSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        public async Task<object> Create(GuestHotelDto guestHotelDto)
        {
            try
            {
                var guestHotelEntity = ObjectMapper.Map<GuestHotel>(guestHotelDto);
                await _guestHotelRepository.InsertAsync(guestHotelEntity);

                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.InsertSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        public async Task<object> Update(GuestHotelDto guestHotelDto)
        {
            try
            {
                var guestHotelEntity = ObjectMapper.Map<GuestHotel>(guestHotelDto);
                await _guestHotelRepository.UpdateAsync(guestHotelEntity);

                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.UpdateSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        public async Task<object> Delete(GuestHotelDto guestHotelDto)
        {
            try
            {
                var guestHotelEntity = ObjectMapper.Map<GuestHotel>(guestHotelDto);
                await _guestHotelRepository.DeleteAsync(guestHotelEntity);

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
