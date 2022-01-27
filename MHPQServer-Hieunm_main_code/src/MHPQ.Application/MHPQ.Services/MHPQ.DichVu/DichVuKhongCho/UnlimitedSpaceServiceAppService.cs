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
    public class UnlimitedSpaceServiceAppService : MHPQAppServiceBase, IUnlimitedSpaceServiceAppService
    {
        private readonly IRepository<UnlimitedSpaceServices, long> _repository;

        public UnlimitedSpaceServiceAppService(IRepository<UnlimitedSpaceServices, long> repository)
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
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<object> Create(UnlimitedSpaceServiceDto dto)
        {
            try
            {
                var entity = new UnlimitedSpaceServices
                {
                    ServiceName = dto.ServiceName,
                    ServiceType = dto.ServiceType,
                    Infor = dto.Infor,
                    PriceHours = dto.PriceHours,
                    TimeTable = dto.TimeTable,
                    
                };


                await _repository.InsertAsync(entity);

                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.InsertSuccess);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<object> Update(UnlimitedSpaceServiceDto dto)
        {
            try
            {
                var entity = new UnlimitedSpaceServices
                {
                    Id = dto.UnlimitedSpaceServiceId,
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
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<object> Delete(UnlimitedSpaceServiceDto dto)
        {
            try
            {
                await _repository.DeleteAsync(dto.UnlimitedSpaceServiceId);

                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.DeleteSuccess);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
