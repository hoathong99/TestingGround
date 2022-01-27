using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Services.DichVu.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu
{
    public class FootballServiceAppService : MHPQAppServiceBase, IFootballServiceAppService
    {

        private readonly IRepository<FootballPitch, long> _footballPitchRepo;
        private readonly IRepository<PitchBooking, long> _pitchBookingRepo;

        public FootballServiceAppService(
            IRepository<FootballPitch, long> footballPitchRepo,
            IRepository<PitchBooking, long> pitchBookingRepo)
        {
            _footballPitchRepo = footballPitchRepo;
            _pitchBookingRepo = pitchBookingRepo;
        }

        public async Task<object> GetAllPitches()
        {
            try
            {
                var result = await _footballPitchRepo.GetAllListAsync();
                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.GetAllSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message); ;
            }

        }


        public async Task<object> GetAllPitchBooking()
        {
            try
            {
                var result = await _pitchBookingRepo.GetAllListAsync();
                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.GetAllSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        /// <summary>
        /// Get all bookings are reserved before
        /// </summary>
        /// <param name="pitchId"></param>
        /// <returns></returns>
        public async Task<object> GetAllBookingOfPitchToday(long? pitchId)
        {
            try
            {
                var result = (from booking in _pitchBookingRepo.GetAll()
                             where booking.FootballPitch.Id == pitchId
                             && booking.ReserveDay.Value.Date == DateTime.Now.Date
                             select booking).ToList();
                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.GetAllSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }

        public async Task<object> UpdateFootballPitch(FootballPitchDto dto)
        {
            try
            {
                await _footballPitchRepo.UpdateAsync(dto);
                return DataResult.ResultSucces(dto, Common.Resource.QuanLyChung.UpdateSuccess);
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        public async Task<object> CreateBooking(PitchBookingDto dto)
        {
            try
            {
                //Tạo mới booking
                var entity = ObjectMapper.Map<PitchBooking>(dto);
                await _pitchBookingRepo.InsertAsync(entity);

                //Cập nhật trạng thái sân
                //var pitchUpdate = await _footballPitchRepo.GetAsync(dto.PitchId);
                //pitchUpdate.State = !pitchUpdate.State;
                //await _footballPitchRepo.UpdateAsync(pitchUpdate);

                return DataResult.ResultSucces(Common.Resource.QuanLyChung.InsertSuccess);
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
    }
}
