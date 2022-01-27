using Abp.Application.Services;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using MHPQ.Authorization;
using MHPQ.Authorization.Users;
using MHPQ.Common.DataResult;
using MHPQ.Common.Enum;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using MHPQ.Users.Dto;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface ISmartHomeAppService : IApplicationService
    {
        //Task<object> CreateOrUpdateSmartHomeAsync(SmartHomeDto input);
        Task<object> CreateSmartHome(CreateSmartHomeInput input);
        Task<object> UpdateSmartHome(UpdateSmartHomeInput input);
        Task<object> CreateOrUpdateFloor(FloorDto input);

        Task<object> GetListUserTenant();
        //Task<object> UpdateSmartHome(SmartHomeDto input);
        //Task<object> CreateOrUpdateRoom(RoomUpdate input);
        //Task<object> GetAllFloor(long smarthomeid);
        //Task<object> GetSettingSmartHome(string code);
        //Task<object> DeleteFloor(long id);
        //Task<object> GetAllRoom(GetRoomInput input);
        //Task<object> DeleteRoom(long id);
        Task<object> GetAllSmartHome();
        Task<object> GetByIdSmartHomeAsync(long id);
        Task<object> GetSettingSmartHomeAsync(string code);
        Task<object> GetBackupSettingSmartHomeAsync(string code);
        Task<object> SearchSmartHomeAsync(DataSearch text);
        Task<object> DeleteSmartHome(long id);

    }


    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class SmartHomeAppService : MHPQAppServiceBase, ISmartHomeAppService
    {
        private readonly IRepository<HomeDevice, long> _homeDeviceRepos;
        private readonly IRepository<Device, long> _deviceRepos;
        private readonly IRepository<DeviceApi, long> _deviceApiRepos;
        private readonly IRepository<SmarthomeApi, long> _smarthomeApiRepos;
        private readonly IRepository<SmartHome, long> _smartHomeRepos;
        private readonly IRepository<HomeServer, long> _homeServerRepos;
        private readonly IRepository<Theme, long> _themeRepos;
        private readonly IRepository<FloorSmartHome, long> _floorSmarHomeRepos;
        private readonly IRepository<RoomSmartHome, long> _roomSmarHomeRepos;
        private readonly IRepository<HomeGateway, long> _homeGatewayRepos;
        private readonly IRepository<HouseOwner, long> _houseOwnerRepos;
        private readonly UserManager _userManager;
        private readonly UserStore _store;


        public SmartHomeAppService(
            UserStore store,
            IRepository<HomeDevice, long> homeDeviceRepos,
            IRepository<Device, long> deviceRepos,
            IRepository<LightDevice, long> lightDeviceRepos,
            IRepository<SecurityDevice, long> securityDeviceRepos,
            IRepository<SmartHome, long> smartHomeRepos,
            IRepository<HomeServer, long> homeServerRepos,
            IRepository<HomeGateway, long> homeGatewayRepos,
            IRepository<Theme, long> themeRepos,
            IRepository<FloorSmartHome, long> floorSmarHomeRepos,
            IRepository<RoomSmartHome, long> roomSmarHomeRepos,
            IRepository<DeviceApi, long> deviceApiRepos,
            IRepository<SmarthomeApi, long> smarthomeApiRepos,
            IRepository<CurtainDeivce, long> curtainDeviceRepos,
            IRepository<ConditionerDevice, long> conditionerDeviceRepos,
            IRepository<DoorEntryDevice, long> doorEntryDeviceRepos,
            IRepository<HouseOwner, long> houseOwnerRepos,
            UserManager userManager
        )
        {
            _homeDeviceRepos = homeDeviceRepos;
            _deviceRepos = deviceRepos;
            _floorSmarHomeRepos = floorSmarHomeRepos;
            _homeServerRepos = homeServerRepos;
            _smartHomeRepos = smartHomeRepos;
            _themeRepos = themeRepos;
            _roomSmarHomeRepos = roomSmarHomeRepos;
            _homeGatewayRepos = homeGatewayRepos;
            _deviceApiRepos = deviceApiRepos;
            _smarthomeApiRepos = smarthomeApiRepos;
            _houseOwnerRepos = houseOwnerRepos;
            _userManager = userManager;
            _store = store;
        }

        #region SmartHome
        //[Obsolete]
        //public async Task<object> CreateOrUpdateSmartHomeAsync(SmartHomeDto input)
        //{

        //    try
        //    {
        //        long t1 = TimeUtils.GetNanoseconds();

        //        input.TenantId = AbpSession.TenantId;
        //        if (input.Id > 0)
        //        {
        //            //update
        //            var updateData = await _smartHomeRepos.GetAsync(input.Id);
        //            if(updateData != null)
        //            {
        //                var oldProp = updateData.Properties;
        //                input.MapTo(updateData);
        //                updateData.PropertiesHistory = oldProp;

        //                //call back
        //                await _smartHomeRepos.UpdateAsync(updateData);
        //            }
        //            mb.statisticMetris(t1, 0, "Ud_smh");

        //            return 0;
        //        }
        //        else
        //        {
        //            //Insert
        //            var insertInput = input.MapTo<SmartHome>();
        //            long id = await _smartHomeRepos.InsertAndGetIdAsync(insertInput);
        //            if(id > 0)
        //            {     
        //                insertInput.SmartHomeCode = GetUniqueKey();
        //                insertInput.Id = id;
        //            }   

        //            //if(input.ListRoom != null)
        //            //{
        //            //    foreach( string name in input.ListRoom)
        //            //    {
        //            //        RoomSmartHome room = new RoomSmartHome()
        //            //        {
        //            //            Name = name,
        //            //            SmartHomeId = id,
        //            //            TenantId = AbpSession.TenantId
        //            //        };
        //            //       await _roomSmarHomeRepos.InsertAsync(room);
        //            //    }
        //            //}
        //           // Logger.Info("input time t1" + t1);
        //            mb.statisticMetris(t1, 0, "is_smh");
        //            var data = DataResult.ResultSucces(insertInput, "Insert success !");
        //            return data;
        //        }


        //    }
        //    catch(Exception e)
        //    {
        //        var data = DataResult.ResultError(e.ToString(), "Có lỗi");
        //        Logger.Fatal(e.Message);
        //        return data;
        //    }
        //}

        public async Task<object> CreateSmartHome(CreateSmartHomeInput input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                var insertInput = new SmartHomeDto();
                insertInput.TenantId = AbpSession.TenantId;
                insertInput.Properties = input.Properties;
                insertInput.SmartHomeCode = GetUniqueKey();
                insertInput.CreatorUserId = input.UserId;
                if (!input.Properties.IsNullOrEmpty())
                {
                    dynamic home = JObject.Parse(input.Properties);
                    try
                    {
                        insertInput.ImageUrl = home.home_infor.img;
                        home.home_infor.home_code = insertInput.SmartHomeCode;
                    }
                    catch (RuntimeBinderException er)
                    {
                        var dt = DataResult.ResultCode(er.Message, "Format properties error !", 415);
                        return dt;
                    }
                    insertInput.Properties = Convert.ToString(home);
                }

                long id = await _smartHomeRepos.InsertAndGetIdAsync(insertInput);
                if(input.UserId > 0 && id > 0)
                {
                    insertInput.Id = id;
                    var member = new HouseOwner()
                    {
                        SmartHomeCode = insertInput.SmartHomeCode,
                        IsAdmin = true,
                        UserId = input.UserId,
                        SmartHomeId = id,
                        TenantId = insertInput.TenantId
                    };

                    await _houseOwnerRepos.InsertAsync(member);
                }
                await CurrentUnitOfWork.SaveChangesAsync();
                if (id > 0)
                {        
                    insertInput.Id = id;
                }

                mb.statisticMetris(t1, 0, "is_smh");
                var data = DataResult.ResultSucces(insertInput, "Insert success !");
                return data;

            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> UpdateSmartHome(UpdateSmartHomeInput input)
        {
            try
            {
                //update
                var updateData = await _smartHomeRepos.FirstOrDefaultAsync(x => (x.TenantId == AbpSession.TenantId) && (x.SmartHomeCode == input.SmartHomeCode));
              
                if (updateData != null)
                {
                    var oldProp = updateData.Properties;
                    updateData.Properties = input.Properties;
                    updateData.PropertiesHistory = oldProp;
                    await _smartHomeRepos.UpdateAsync(updateData);  
                   
                }

                return 0;


            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                Logger.Fatal(e.Message);
                return data;
            }
        }


        public async Task<object> GetAllSmartHome()
        {
            try
            {
                var result = await _smartHomeRepos.GetAllListAsync();

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }

        }

        [Obsolete]
        public IEnumerable<TypeDeviceInput> GetAllTypeDevice(RoomUpdate room, string type) 
        {

            var lightDevices = (from device in _homeDeviceRepos.GetAll()
                                where device.RoomId == room.Id && device.GroupDevice == type || (AbpSession.TenantId != null && device.TenantId == AbpSession.TenantId)
                                select new TypeDeviceInput()
                                {
                                    Id = device.Id,
                                    TypeDevice = type,
                                    DeviceHomeId = device.HomeDeviceId,
                                    DeviceId = device.DeviceSettingId,
                                    DeviceNumber = device.DeviceNumber.Value,
                                    Depscription = device.HomeDeviceAddress,
                                    DeviceGateway = (from gate in _homeGatewayRepos.GetAll()
                                                     where gate.Id == device.HomeServerId
                                                     select gate).FirstOrDefault()
                                }).AsQueryable();
                               
            return lightDevices.ToList();
        }

        [Obsolete]
        public async Task<object> GetListUserTenant()
        {
            try
            {
                var users = await _store.GetAllUserTenantAsync();
                var result = ObjectMapper.Map<List<UserDto>>(users);


                var data = DataResult.ResultSucces(result, "Get success!");
                return data;

            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                return data;
            }
        }

        public async Task<object> GetByIdSmartHomeAsync(long id)
        {
            try
            {
                var smarthome = await _smartHomeRepos.FirstOrDefaultAsync(x => x.Id == id);
                //var rooms = _roomSmarHomeRepos.GetAllList(room => room.SmartHomeId == smarthome.Id);

                //setting.Rooms = rooms.MapTo<List<RoomUpdate>>();

                //if (setting.Rooms != null)
                //{
                //    setting.Rooms.ForEach(room => {
                //        room.LightingIds = GetAllTypeDevice(room, AppConsts.LightingDevice).MapTo<List<TypeLightingDevice>>();
                //        room.CurtainIds = GetAllTypeDevice(room, AppConsts.CurtainDevice).MapTo<List<TypeCurtainDevice>>();
                //        room.AirIds = GetAllTypeDevice(room, AppConsts.AirDevice).MapTo<List<TypeAirDevice>>();
                //        room.ConditionerIds = GetAllTypeDevice(room, AppConsts.ConditionerDevice).MapTo<List<TypeConditionerDevice>>();
                //        room.ConnectionIds = GetAllTypeDevice(room, AppConsts.ConnectionDevice).MapTo<List<TypeConnectionDevice>>();
                //        room.DoorEntryIds = GetAllTypeDevice(room, AppConsts.DoorEntryDevice).MapTo<List<TypeDoorEntryDevice>>();
                //        room.FireAlarmIds = GetAllTypeDevice(room, AppConsts.FireAlarmDevice).MapTo<List<TypeFireAlarmDevice>>();
                //        room.WatterIds = GetAllTypeDevice(room, AppConsts.WaterDevice).MapTo<List<TypeWaterDevice>>();
                //        room.SecurityIds = GetAllTypeDevice(room, AppConsts.SecurityDevice).MapTo<List<TypeSecurityDevice>>();
                //    });
                //};
                var data = DataResult.ResultSucces(smarthome, "Get success");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;

            }
        }

        public async Task<object> GetSettingSmartHomeAsync(string code)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                    
                var setting = new SmartHomeSettingOutput();
                var smarthome = await _smartHomeRepos.FirstOrDefaultAsync(x =>  (x.TenantId == AbpSession.TenantId) && (x.SmartHomeCode == code));
                setting.SmartHomecode = smarthome.SmartHomeCode;
                setting.Properties = smarthome.Properties;
                //var homeserver = _homeGatewayRepos.FirstOrDefault(x => ( x.TenantId == AbpSession.TenantId) && (x.SmartHomeId == smarthome.Id));
              
                //var rooms = _roomSmarHomeRepos.GetAllList(room =>  room.TenantId == AbpSession.TenantId && room.SmartHomeId == smarthome.Id);
                //setting.Rooms = rooms.MapTo<List<RoomUpdate>>();

                //if (rooms != null)
                //{
                //    setting.Rooms.ForEach(room =>
                //    {
                //        var homedevices = _homeDeviceRepos.GetAllList(d => d.RoomId == room.Id);
                //        room.Devices = homedevices.MapTo<List<HomeDeviceDto>>();

                //        room.Devices.ForEach(device =>
                //        {
                //            var apis = _smarthomeApiRepos.GetAllList(a => a.DeviceSmarthomeId == device.Id);
                //            device.ListApis = apis;
                //        });
                //    });
                //}
                stopwatch.Stop();
                var timecount = stopwatch.ElapsedMilliseconds;
                Logger.Info(timecount.ToString());
                var data = DataResult.ResultSucces(setting, "Get success");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                Logger.Fatal(e.Message);
                return data;

            }
        }

        public async Task<object> GetBackupSettingSmartHomeAsync(string code)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                var setting = new SmartHomeSettingOutput();
                var smarthome = await _smartHomeRepos.FirstOrDefaultAsync(x => (x.TenantId == AbpSession.TenantId) && (x.SmartHomeCode == code));
                setting.SmartHomecode = smarthome.SmartHomeCode;
                setting.Properties = smarthome.PropertiesHistory;
                stopwatch.Stop();
                var timecount = stopwatch.ElapsedMilliseconds;
                Logger.Info(timecount.ToString());
                var data = DataResult.ResultSucces(setting, "Get success");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                Logger.Fatal(e.Message);
                return data;

            }
        }

        [Obsolete]
        public void AddOrUpdateHomeDevice(RoomDto room, TypeDeviceInput type)
        {
            var device = _deviceRepos.Get(type.DeviceId);
            
            if (device != null && type.Id == 0 && !type.IsDelete)
            {
                //var apis = _deviceApiRepos.GetAllList(x => (x.DeviceId == device.Id && x.HomeServerId == type.DeviceGateway.Id));
                var lighting = new HomeDevice()
                {
                    SmartHomeId = room.SmartHomeId,
                    RoomId = room.Id,
                    FloorId = room.FloorSmartHomeId,
                    Name = device.Name,
                    DeviceCode = device.DeviceCode,
                    Url = device.Url,
                    Producer = device.Producer,
                    Parameters = device.Parameters,
                    Port = device.Port,
                    TypeDevice = device.TypeDevice,
                    HomeDeviceId = type.DeviceHomeId,
                    HomeServerId = type.DeviceGateway != null? type.DeviceGateway.Id : 0 ,
                    ImageUrl = device.ImageUrl,
                    DeviceSettingId = device.Id,
                    DeviceNumber = type.DeviceNumber,
                    HomeDeviceAddress = type.Depscription,
                    GroupDevice = type.GroupDevice
                };

                _homeDeviceRepos.Insert(lighting);

                //if (apis != null)
                //{
                //    apis.ForEach(api =>
                //    {
                //        var homeApi = new SmarthomeApi()
                //        {
                //            HomeServerId = type.DeviceGateway.Id,
                //            ContentType = api.ContentType,
                //            DeviceId = api.DeviceId,
                //            DeviceName = api.DeviceName,
                //            DeviceSmarthomeId = lightingId,
                //            Method = api.Method,
                //            Name = api.Name,
                //            Url = api.Url,
                //            Port = api.Port,
                //            Values = api.Values,
                //            GateWay = api.Gateway,
                //            SmarthomeId = room.SmartHomeId,
                //            HomeServerName = type.DeviceGateway.Name
                //        };
                //        var apiId = _smarthomeApiRepos.InsertAndGetId(homeApi);
                //    });
                //}
                //else
                //{
                //    var homeApi = new SmarthomeApi()
                //    {
                //        HomeServerId = type.DeviceGateway.Id,
                //        DeviceId = type.DeviceId,
                //        DeviceSmarthomeId = lightingId,
                //        //Name = "Lighting",
                //        SmarthomeId = room.SmartHomeId,
                //        HomeServerName = type.DeviceGateway.Name
                //    };
                //    var apiId = _smarthomeApiRepos.InsertAndGetId(homeApi);
                //}
            }
            else if(device != null && type.Id > 0 && !type.IsDelete)
            {
                var homedevice =  _homeDeviceRepos.Get(type.Id);
                if(homedevice != null)
                {
                    homedevice.HomeDeviceId = type.DeviceHomeId;
                    homedevice.DeviceSettingId = type.DeviceId;
                    homedevice.DeviceNumber = type.DeviceNumber;
                    homedevice.HomeServerId = type.DeviceGateway != null ? type.DeviceGateway.Id : 0;
                    homedevice.HomeDeviceAddress = type.Depscription;
                    _homeDeviceRepos.Update(homedevice);
                }
            }
            else if (device != null && type.Id > 0 && type.IsDelete)
            {
                var homedevice = _homeDeviceRepos.Get(type.Id);
                if (homedevice != null)
                {                   
                    _homeDeviceRepos.Delete(homedevice);
                }
            }
           
        }

        [Obsolete]
        public void UpdateDeviceSmartHome(RoomUpdate room)
        {
            try
            {
                
                if (room.IsSmartLighting.Value)
                {
                    if(room.LightingIds != null)
                    {
                        foreach(var type in room.LightingIds)
                        {
                            type.GroupDevice = AppConsts.LightingDevice;
                            AddOrUpdateHomeDevice(room, type);
                        }
                    }

                }
                if (room.IsSmartCurtain.Value)
                {
                    if (room.CurtainIds != null)
                    {
                        foreach (var type in room.CurtainIds)
                        {
                            type.GroupDevice = AppConsts.CurtainDevice;
                            AddOrUpdateHomeDevice(room, type);
                        }
                    }

                }
                if (room.IsSmartAir.Value)
                {
                    if (room.AirIds != null)
                    {
                        foreach (var type in room.AirIds)
                        {
                            type.GroupDevice = AppConsts.AirDevice;
                            AddOrUpdateHomeDevice(room, type);
                        }
                    }


                }
                if (room.IsSmartConditioner.Value)
                {
                    if (room.ConditionerIds != null)
                    {
                        foreach (var type in room.ConditionerIds)
                        {
                            type.GroupDevice = AppConsts.ConditionerDevice;
                            AddOrUpdateHomeDevice(room, type);
                        }
                    }
                }
                if (room.IsSmartConnection.Value)
                {
                    if (room.ConnectionIds != null)
                    {
                        foreach (var type in room.ConnectionIds)
                        {
                            type.GroupDevice = AppConsts.ConnectionDevice;
                            AddOrUpdateHomeDevice(room, type);
                        }
                    }


                }
                if (room.IsSmartDoorEntry.Value)
                {
                    if (room.DoorEntryIds != null)
                    {
                        foreach (var type in room.DoorEntryIds)
                        {
                            type.GroupDevice = AppConsts.DoorEntryDevice;
                            AddOrUpdateHomeDevice(room, type);
                        }
                    }


                }
                if (room.IsSmartFireAlarm.Value)
                {
                    if (room.FireAlarmIds != null)
                    {
                        foreach (var type in room.FireAlarmIds)
                        {
                            type.GroupDevice = AppConsts.FireAlarmDevice;
                            AddOrUpdateHomeDevice(room, type);
                        }
                    }


                }
                if (room.IsSmartSecurity.Value)
                {
                    if (room.SecurityIds != null)
                    {
                        foreach (var type in room.SecurityIds)
                        {
                            type.GroupDevice = AppConsts.SecurityDevice;
                            AddOrUpdateHomeDevice(room, type);
                        }
                    }

                }
                if (room.IsSmartWatter.Value)
                {
                    if (room.WatterIds != null)
                    {
                        foreach (var type in room.WatterIds)
                        {
                            type.GroupDevice = AppConsts.WaterDevice;
                            AddOrUpdateHomeDevice(room, type);
                        }
                    }

                }

            }
            catch(Exception ex)
            {
                Logger.Fatal(ex.Message);

            }
        }



        public async Task<object> SearchSmartHomeAsync(DataSearch text)
        {
            try
            {
                var result = await _smartHomeRepos.FirstOrDefaultAsync(x => x.SmartHomeCode == text.data);

                var deviceApi = await _smarthomeApiRepos.GetAllListAsync(x => x.SmarthomeId == result.Id);

                var data = new
                {
                    smarthome = result,
                    api = deviceApi
                };
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                Logger.Fatal(e.Message);
                return data;

            }
        }

        public async Task<object> DeleteSmartHome(long id)
        {
            try
            {
                var smarthome = await _smartHomeRepos.GetAsync(id);
                if (smarthome != null)
                {
                    await _smartHomeRepos.DeleteAsync(smarthome);
                    var data = DataResult.ResultSucces("Xóa thành công !");
                    return data;
                }
                else
                {
                    var data = DataResult.ResultFail("Smarthome không tồn tại !");
                    return data;
                }
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                Logger.Fatal(e.Message);
                return data;
            }
        }
        #endregion

        #region Floor
        [Obsolete]
        public async Task<object> CreateOrUpdateFloor(FloorDto input)
        {
            try
            {
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _floorSmarHomeRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);
                        await _floorSmarHomeRepos.UpdateAsync(updateData);
                        
                    }
                    var data = DataResult.ResultSucces(updateData, "Get success");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<FloorSmartHome>();
                    long id = await _floorSmarHomeRepos.InsertAndGetIdAsync(insertInput);
                    var data = DataResult.ResultSucces(insertInput, "Get success");
                    return data;
                }

               
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        [Obsolete]
        public async Task<object> GetAllFloor(long smarthomeid)
        {
            try
            {
                var result = await _floorSmarHomeRepos.GetAllListAsync(x => x.SmartHomeId == smarthomeid);

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }
        }

        [Obsolete]
        public async Task<object> DeleteFloor(long id)
        {
            try
            {
                var floor = await _floorSmarHomeRepos.GetAsync(id);
                if (floor != null)
                {
                    await _floorSmarHomeRepos.DeleteAsync(floor);
                    var data = DataResult.ResultSucces("Xóa thành công !");
                    return data;
                }
                else
                {
                    var data = DataResult.ResultFail("Tầng không tồn tại !");
                    return data;
                }
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }
        }
        #endregion

        #region Room
        [Obsolete]
        public async Task<object> CreateOrUpdateRoom(RoomUpdate input)
        {
            try
            {
                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _roomSmarHomeRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);
                        await _roomSmarHomeRepos.UpdateAsync(updateData);

                    }
                    UpdateDeviceSmartHome(input);
                    var data = DataResult.ResultSucces(updateData, "Get success");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<RoomSmartHome>();
                    long id = await _roomSmarHomeRepos.InsertAndGetIdAsync(insertInput);
                    UpdateDeviceSmartHome(input);
                    var data = DataResult.ResultSucces(insertInput, "Get success");
                    return data;

                }

                
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        [Obsolete]
        public async Task<object> GetAllRoom(GetRoomInput input)
        {
            try
            {
                //var query = (from room in _roomSmarHomeRepos.GetAll()
                //             select new RoomOutputDto()
                //             {
                //                 Id = room.Id,
                //                 CreationTime = room.CreationTime,
                //                 CreatorUserId = room.CreatorUserId,
                //                 FloorSmartHomeId = room.FloorSmartHomeId,
                //                 ImageUrl = room.ImageUrl,
                //                 Name = room.Name,
                //                 Number = room.Number,
                //                 NumberDevices = room.NumberDevices,
                //                 SmartHomeId = room.SmartHomeId
                //             });

                if(input.FormCase == (int)FORM_CASE_ROOM.ROOM_IN_HOME)
                {
                    var result = await _roomSmarHomeRepos.GetAllListAsync(x => x.SmartHomeId == input.SmartHomeId);
                    var data = DataResult.ResultSucces(result, "Get success!");
                    return data;
                }
                else
                {
                    var result = await _roomSmarHomeRepos.GetAllListAsync(x => x.FloorSmartHomeId == input.FloorSmartHomeId);
                    var data = DataResult.ResultSucces(result, "Get success!");
                    return data;
                }

                
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }
        }

        [Obsolete]
        public async Task<object> DeleteRoom(long id)
        {
            try
            {
                var room = await _roomSmarHomeRepos.GetAsync(id);
                if (room != null)
                {
                    await _roomSmarHomeRepos.DeleteAsync(room);
                    var data = DataResult.ResultSucces("Xóa thành công !");
                    return data;
                }
                else
                {
                    var data = DataResult.ResultFail("Tầng không tồn tại !");
                    return data;
                }
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }
        }


        #endregion


        #region Function

        [Obsolete]
        public IDevice MapDeviceOptions(Device device, IDevice x, RoomSmartHome room)
        {
            x.SmartHomeId = room.SmartHomeId;
            //x.RoomId = room.Id;
            //x.FloorId = room.FloorSmartHomeId;
            //x.Name = device.Name;
            //x.DeviceCode = device.DeviceCode;
            //x.Url = device.Url;
            //x.Producer = device.Producer;
            //x.Parameters = device.Parameters;
            //x.Port = device.Port;
            //x.TypeDevice = device.TypeDevice;
            //x.HomeserverAvailables = device.HomeserverAvailables;
            //x.HomeDeviceId = device.HomeDeviceId;
            //x.HomeServerId = room.SmartHomeId;
            return x;
        }


        #endregion

    }
}
