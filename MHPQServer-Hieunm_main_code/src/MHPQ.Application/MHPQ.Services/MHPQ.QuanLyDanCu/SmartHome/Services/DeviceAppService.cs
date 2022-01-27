using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface IDeviceAppService : IApplicationService
    {

        #region Device
        Task<object> CreateOrUpdateDevice(DeviceDto input);
        Task<object> GetAllDevice();
        Task<object> GetAllDeviceByHomeId(long smarthomeid);
        Task<object> GetByIdDevice(long id);
        Task<object> DeleteDevice(long id);
        //IEnumerable<string> GetAllDeviceType();
        //IEnumerable<Device> GetAllDeviceByType(string type);
        #endregion
       

    }

    public class DeviceAppService : MHPQAppServiceBase, IDeviceAppService
    {

        private readonly IRepository<Device, long> _deviceRepos;
        private readonly IRepository<SmartHome, long> _smartHomeRepos;
        private readonly IRepository<HomeServer, long> _homeServerRepos;
        private readonly IRepository<Theme, long> _themeRepos;
        private readonly IRepository<FloorSmartHome, long> _floorSmarHomeRepos;
        private readonly IRepository<RoomSmartHome, long> _roomSmarHomeRepos;

        public DeviceAppService(

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
        public async Task<object> CreateOrUpdateDevice(DeviceDto input)
        {

            try
            {
                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _deviceRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);
                        await _deviceRepos.UpdateAsync(updateData);

                    }
                    return 1;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<Device>();
                    long id = await _deviceRepos.InsertAndGetIdAsync(insertInput);
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

        public async Task<object> GetAllDevice()
        {
            try
            {
                var a = AbpSession.TenantId; 
                var result = await _deviceRepos.GetAllListAsync();

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }

        }

        public async Task<object> GetAllDeviceByHomeId(long smarthomeid)
        {
            try
            {
                var result = await _deviceRepos.GetAllListAsync();

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }

        }

        public async Task<object> GetByIdDevice(long id)
        {
            try
            {
                var result = await _deviceRepos.GetAsync(id);

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;

            }
        }

        public async Task<object> DeleteDevice(long id)
        {
            try
            {
                var device = await _deviceRepos.GetAsync(id);
                if (device != null)
                {
                    await _deviceRepos.DeleteAsync(device);
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

        //public IEnumerable<string> GetAllDeviceType()
        //{
        //    return (from device in _deviceRepos.GetAllList()
        //            select device.TypeDevice).Distinct().ToList();

        //}

        //public IEnumerable<Device> GetAllDeviceByType(string type) {
        //    return (from device in _deviceRepos.GetAll()
        //            where device.TypeDevice == type
        //            select device).ToList();
        //}

    }
}
