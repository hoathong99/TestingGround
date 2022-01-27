using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan
{
    public class HotelAppService : MHPQAppServiceBase, IHotelAppService
    {
        private readonly IRepository<Hotel, long> _hotelRepository;

        public HotelAppService(IRepository<Hotel, long> hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<object> GetHotels()
        {
            try
            {
                var result = await _hotelRepository.GetAllListAsync();
                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.GetAllSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }

        }
        public async Task<object> CreateHotel(HotelDto hotelDto)
        {
            try
            {
                var hotelEntity = ObjectMapper.Map<Hotel>(hotelDto);
                await _hotelRepository.InsertAsync(hotelEntity);

                var result = await _hotelRepository.GetAllListAsync();
                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.InsertSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);               
            }
        }
        public async Task<object> UpdateHotel(HotelDto hotelDto)
        {
            try
            {
                var hotelEntity = ObjectMapper.Map<Hotel>(hotelDto);
                await _hotelRepository.UpdateAsync(hotelEntity);

                var result = await _hotelRepository.GetAllListAsync();
                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.UpdateSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        public async Task<object> DeleteHotel(HotelDto hotelDto)
        {
            try
            {
                var hotelEntity = ObjectMapper.Map<Hotel>(hotelDto);
                await _hotelRepository.DeleteAsync(hotelEntity);

                var result = await _hotelRepository.GetAllListAsync();
                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.DeleteSuccess);
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
