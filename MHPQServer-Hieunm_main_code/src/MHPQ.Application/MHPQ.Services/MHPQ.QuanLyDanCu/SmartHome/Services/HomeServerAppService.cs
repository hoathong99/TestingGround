

using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using System;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface IHomeServerAppService : IApplicationService
    {
        Task<object> CreateOrUpdateHomeServer(HomeServerDto input);
        Task<object> GetAllHomeServer();
        Task<object> GetByIdHomeServer(long id);
        Task<object> DeleteHomeServer(long id);
    }

    public class HomeServerAppService : MHPQAppServiceBase, IHomeServerAppService
    {

        private readonly IRepository<Device, long> _deviceRepos;
        private readonly IRepository<SmartHome, long> _smartHomeRepos;
        private readonly IRepository<HomeServer, long> _homeServerRepos;
        private readonly IRepository<Theme, long> _themeRepos;
        private readonly IRepository<FloorSmartHome, long> _floorSmarHomeRepos;
        private readonly IRepository<RoomSmartHome, long> _roomSmarHomeRepos;

        public HomeServerAppService(

            IRepository<Device, long> deviceRepos,
            IRepository<SmartHome, long> smartHomeRepos,
            IRepository<HomeServer, long> homeServerRepos,
            IRepository<Theme, long> themeRepos,
            IRepository<FloorSmartHome, long> floorSmarHomeRepos,
            IRepository<RoomSmartHome, long> roomSmarHomeRepos
        )
        {
            _deviceRepos = deviceRepos;
            _floorSmarHomeRepos = floorSmarHomeRepos;
            _homeServerRepos = homeServerRepos;
            _smartHomeRepos = smartHomeRepos;
            _themeRepos = themeRepos;
            _roomSmarHomeRepos = roomSmarHomeRepos;
        }

        [Obsolete]
        public async Task<object> CreateOrUpdateHomeServer(HomeServerDto input)
        {

            try
            {
                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _homeServerRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);
                        await _homeServerRepos.UpdateAsync(updateData);
                       
                    }
                    return 1;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<HomeServer>();
                    long id = await _homeServerRepos.InsertAndGetIdAsync(insertInput);
                    if (id > 0)
                    {
                        insertInput.Id = id;
                    }
                    return insertInput;
                }

                
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public async Task<object> GetAllHomeServer()
        {
            try
            {
                var result = await _homeServerRepos.GetAllListAsync();

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }

        }

        public async Task<object> GetByIdHomeServer(long id)
        {
            try
            {
                var result = await _homeServerRepos.GetAsync(id);

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;

            }
        }

        public async Task<object> DeleteHomeServer(long id)
        {
            try
            {
                var device = await _homeServerRepos.GetAsync(id);
                if (device != null)
                {
                    await _homeServerRepos.DeleteAsync(device);
                    var data = DataResult.ResultSucces("Xóa thành công !");
                    return data;
                }
                else
                {
                    var data = DataResult.ResultFail("Thiết bị không tồn tại !");
                    return data;
                }
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }
        }
    }
}
