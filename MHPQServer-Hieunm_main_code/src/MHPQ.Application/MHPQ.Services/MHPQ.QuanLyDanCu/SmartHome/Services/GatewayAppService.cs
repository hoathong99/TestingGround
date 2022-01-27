using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Service.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{

    public interface IHomeGatewayAppService : IApplicationService
    {
        Task<object> CreateOrUpdateGateway(HomeGatewayDto input);
        Task<object> GetAllGateway();
        Task<object> GetAllGatewayByHomeId(long id);
        Task<object> GetByIdGateway(long id);
        Task<object> DeleteGateway(long id);
    }


    public class HomeGatewayAppService : MHPQAppServiceBase, IHomeGatewayAppService
    {


        private readonly IRepository<Device, long> _deviceRepos;
        private readonly IRepository<DeviceApi, long> _deviceApiRepos;
        private readonly IRepository<SmartHome, long> _smartHomeRepos;
        private readonly IRepository<HomeServer, long> _homeServerRepos;
        private readonly IRepository<Theme, long> _themeRepos;
        private readonly IRepository<RoomSmartHome, long> _roomSmarHomeRepos;
        private readonly IRepository<HomeGateway, long> _homeGatewayRepos;

        public HomeGatewayAppService(

            IRepository<Device, long> deviceRepos,
            IRepository<SmartHome, long> smartHomeRepos,
            IRepository<HomeServer, long> homeServerRepos,
            IRepository<Theme, long> themeRepos,
            IRepository<RoomSmartHome, long> roomSmarHomeRepos,
            IRepository<DeviceApi, long> deviceApiRepos,
            IRepository<HomeGateway, long> homeGatewayRepos
        )
        {
            _deviceRepos = deviceRepos;
            _homeServerRepos = homeServerRepos;
            _smartHomeRepos = smartHomeRepos;
            _themeRepos = themeRepos;
            _roomSmarHomeRepos = roomSmarHomeRepos;
            _deviceApiRepos = deviceApiRepos;
            _homeGatewayRepos = homeGatewayRepos;
        }

        [Obsolete]
        public async Task<object> CreateOrUpdateGateway(HomeGatewayDto input)
        {

            try
            {
                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _homeGatewayRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);
                        await _homeGatewayRepos.UpdateAsync(updateData);

                    }
                    var data = DataResult.ResultSucces(updateData, " Success!");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<HomeGateway>();

                    long id = await _homeGatewayRepos.InsertAndGetIdAsync(insertInput);
                    if (id > 0)
                    {
                        insertInput.Id = id;
                    }
                    var data = DataResult.ResultSucces(insertInput, "Success!");
                    return data;
                }


            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message, e);
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }
        }

        public async Task<object> GetAllGateway()
        {
            try
            {
                var result = await _homeGatewayRepos.GetAllListAsync();

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message, e);
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }

        }

        public async Task<object> GetAllGatewayByHomeId(long id)
        {
            try
            
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                var result = await _homeGatewayRepos.GetAllListAsync(x => x.SmartHomeId == id  && x.TenantId == AbpSession.TenantId);

                var data = DataResult.ResultSucces(result, "Get success!");
                stopwatch.Stop();
                var timecount = stopwatch.ElapsedMilliseconds;
                Logger.Info(timecount.ToString());
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message, e);
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }

        }

        public async Task<object> GetByIdGateway(long id)
        {
            try
            {
                var result = await _homeGatewayRepos.GetAsync(id);

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message, e);
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;

            }
        }

        public async Task<object> DeleteGateway(long id)
        {
            try
            {
                var device = await _homeGatewayRepos.GetAsync(id);
                if (device != null)
                {
                    await _homeGatewayRepos.DeleteAsync(device);
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
                Logger.Fatal(e.Message, e);
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }
        }
    }
}
