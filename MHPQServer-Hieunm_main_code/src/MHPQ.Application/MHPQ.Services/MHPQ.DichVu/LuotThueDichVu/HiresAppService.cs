using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Services.ThanhToan;
using MHPQ.Sessions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu
{
    public class HiresAppService : MHPQAppServiceBase, IHiresAppService
    {
        private readonly IRepository<Hires, long> _hireRepository;
        private readonly IRepository<LimitedSpaceServices, long> _limitedSpaceRepository;
        private readonly IRepository<UnlimitedSpaceServices, long> _unlimitedSpaceRepository;

        public HiresAppService(
            IRepository<Hires, long> hireRepository,
            IRepository<LimitedSpaceServices, long> limitedSpaceRepository,
            IRepository<UnlimitedSpaceServices, long> unlimitedSpaceRepository)
        {
            _hireRepository = hireRepository;
            _limitedSpaceRepository = limitedSpaceRepository;
            _unlimitedSpaceRepository = unlimitedSpaceRepository;

        }

        public void ChangeEmptySpace()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Thêm lượt thuê dịch vụ ko giới hạn chỗ
        /// </summary>
        /// <param name="hireDto"></param>
        /// <returns></returns>
        public async Task<object> CreateHireUnlimitedSpace(HireServiceDto hireDto)
        {
            try
            {
                

                var hire = new Hires
                {
                    GuestServiceId = hireDto.GuestServiceId,
                    UnlimitedSpaceServiceId = hireDto.UnlimitedSpaceServiceId,
                    HireHours = hireDto.HireHours,
                };

                await _hireRepository.InsertAsync(hire);

                return DataResult.ResultSucces(Common.Resource.QuanLyChung.InsertSuccess);
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return null;
            }
        }
        /// <summary>
        /// thêm lượt thuê dịch vụ giới hạn chỗ
        /// </summary>
        /// <param name="hireDto"></param>
        /// <returns></returns>
        public async Task<object> CreateHireLimitedSpace(HireServiceDto hireDto)
        {
            try
            {
                //check nếu còn empty space thì mới cho insert
                var emptySpaceService = this.GetListEmptySpaceService(hireDto.LimitedSpaceServiceId);
                if(emptySpaceService.Count() > 0)
                {
                    
                    var hire = new Hires
                    {
                        LimitedSpaceServiceId = hireDto.LimitedSpaceServiceId,
                        HireHours = hireDto.HireHours,
                        GuestServiceId = hireDto.GuestServiceId,
                        NumberSpace = hireDto.NumberSpace,
                    };

                    await _hireRepository.InsertAsync(hire);

                    //Update lại số space trong dịch vụ LimitedSpaceService
                    var limitedSpace = await _limitedSpaceRepository.GetAsync(hireDto.LimitedSpaceServiceId);

                    limitedSpace.EmptySpace = limitedSpace.TotalSpace - hireDto.NumberSpace;

                    await _limitedSpaceRepository.UpdateAsync(limitedSpace);

                    return DataResult.ResultSucces(Common.Resource.QuanLyChung.InsertSuccess);
                }

                return DataResult.ResultFail(Common.Resource.QuanLyChung.InsertFail);
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return null;
            }
        }
        /// <summary>
        /// lấy dịch vụ còn chỗ trống theo Id
        /// </summary>
        /// <param name="limitedSpaceServiceId"></param>
        /// <returns></returns>
        public IEnumerable<LimitedSpaceServices> GetListEmptySpaceService(long limitedSpaceServiceId)
        {
            try
            {
                var emptyLimitedSpaceService = (from limitedSpaces in _limitedSpaceRepository.GetAll()
                                                where limitedSpaces.Id == limitedSpaceServiceId
                                                && limitedSpaces.EmptySpace > 0
                                                select limitedSpaces).ToList();

                return emptyLimitedSpaceService;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return null;
            }
        }

        public async Task<DataResult> GetEmptySpaceService(long limitedSpaceServiceId)
        {
            try
            {
                var emptyLimitedSpaceService = this.GetListEmptySpaceService(limitedSpaceServiceId);

                var result = DataResult.ResultSucces(emptyLimitedSpaceService, Common.Resource.QuanLyChung.GetEmptySpaceService);
                return result;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return null;
            }
        }

        public async Task<object> GetAll()
        {
            try
            {
                var data = await _hireRepository.GetAllListAsync();
                var result = DataResult.ResultSucces(data ,Common.Resource.QuanLyChung.GetAllSuccess);
                return result;

            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return null;
            }
        }

    }
}
