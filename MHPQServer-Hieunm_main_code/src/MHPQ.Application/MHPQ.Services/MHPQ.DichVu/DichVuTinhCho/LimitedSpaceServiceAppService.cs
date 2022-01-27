using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu
{
    public class LimitedSpaceServiceAppService : MHPQAppServiceBase, ILimitedSpaceServiceAppService
    {

        private readonly IRepository<LimitedSpaceServices, long> _repository;

        public LimitedSpaceServiceAppService(IRepository<LimitedSpaceServices, long> repository)
        {
            _repository = repository;
        }

        public async Task<object> GetAll()
        {
            try
            {
                var result = await _repository.GetAllListAsync();

                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.GetAllSuccess);
                return data;
            }
            catch (Exception e)
            {

                Logger.Fatal(e.Message);
                return null;
            }

        }
        public async Task<object> Create(LimitedSpaceServiceDto dto)
        {
            try
            {
                var entity = new LimitedSpaceServices
                {
                    ServiceName = dto.ServiceName,
                    ServiceType = dto.ServiceType,
                    Infor = dto.Infor,
                    PriceHours = dto.PriceHours,
                    TimeTable = dto.TimeTable,
                    TotalSpace = dto.TotalSpace,
                    EmptySpace = dto.EmptySpace,
                };


                await _repository.InsertAsync(entity);

                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.InsertSuccess);
                return data;
            }
            catch (Exception e)
            {

                Logger.Fatal(e.Message);
                return null;
            }
        }
        public async Task<object> Update(LimitedSpaceServiceDto dto)
        {
            try
            {
                var entity = new LimitedSpaceServices
                {
                    Id = dto.LimitedSpaceServiceId,
                    ServiceName = dto.ServiceName,
                    ServiceType = dto.ServiceType,
                    Infor = dto.Infor,
                    PriceHours = dto.PriceHours,
                    TimeTable = dto.TimeTable,
                };

                await _repository.UpdateAsync(entity);

                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.UpdateSuccess);
                return data;
            }
            catch (Exception e)
            {

                Logger.Fatal(e.Message);
                return null;
            }
        }
        public async Task<object> Delete(LimitedSpaceServiceDto dto)
        {
            try
            {
                await _repository.DeleteAsync(dto.LimitedSpaceServiceId);

                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.DeleteSuccess);
                return data;
            }
            catch (Exception e)
            {

                Logger.Fatal(e.Message);
                return null;
            }
        }
    }
}
