using Abp.AutoMapper;
using MHPQ.EntityDb;
using MHPQ.Service.Dto;
using System;
using System.Collections.Generic;

namespace MHPQ.Services.Dto
{

    public class GetSettingInputDto
    {
        public long ? Id { get; set; }
        public string Code { get; set; }
    }
    public class SmartHomeInput
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string SmartHomeCode { get; set; }
        public long? UserId { get; set; }
        public string Scope { get; set; }
        public string RefreshToken { get; set; }
        public int? NumberRooms { get; set; }
        public int? NumberFloors { get; set; }
        public int? NumberDevices { get; set; }
        public int? NumberHomeServers { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? CreationTime { get; set; }
    }

    [AutoMap(typeof(SmartHome))]
    public class SmartHomeDto : SmartHome
    {

        //public long? SmarthomeServerId { get; set; }
        //public string HomeServerName { get; set; }
        //public List<string> ListRoom { get; set; }
    }

    public class CreateSmartHomeInput
    {
        public string Properties { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

    public class MemberSmarthomeInput
    {
        public long UserId { get; set; }
        public string SmarthomeCode { get; set; }

    }

    public class AdminCreateSmartHomeInput
    {
        public string Properties { get; set; }
        public long? UserId { get; set; }
    }

    public class UpdateSmartHomeInput
    {
        public string SmartHomeCode { get; set; }
        public string Properties { get; set; }

    }

    public class CreateMemberInput
    {
        public string SmarthomeCode { get; set; }
        public string UserNameOrEmail { get; set; }
    }

    public interface ITypeDeviceInput
    {
        public long Id { get; set; }
        public long DeviceId { get; set; }
        public string DeviceHomeId { get; set; }
        public HomeGateway DeviceGateway { get; set; }
        public int DeviceNumber { get; set; }
        public string TypeDevice { get; set; }
        public bool IsDelete { get; set; }
    }

    public class TypeDeviceInput: ITypeDeviceInput
    {
        public long Id { get; set; }
        public long DeviceId { get; set; }
        public string DeviceHomeId { get; set; }
        public HomeGateway DeviceGateway { get; set; }
        public int DeviceNumber { get; set; }
        public string TypeDevice { get; set; }
        public string GroupDevice { get; set; }
        public bool IsDelete { get; set; }
        public string Depscription { get; set; }

    }

    [AutoMap(typeof(TypeDeviceInput))]
    public class TypeLightingDevice : TypeDeviceInput
    {
        public TypeLightingDevice()
        {
            TypeDevice = AppConsts.LightingDevice;
        }

    }

    [AutoMap(typeof(TypeDeviceInput))]
    public class TypeCurtainDevice : TypeDeviceInput
    {
        public TypeCurtainDevice()
        {
            TypeDevice = AppConsts.CurtainDevice;
        }
    }

    [AutoMap(typeof(TypeDeviceInput))]
    public class TypeAirDevice : TypeDeviceInput
    {
        public TypeAirDevice()
        {
            TypeDevice = AppConsts.AirDevice;
        }
    }

    [AutoMap(typeof(TypeDeviceInput))]
    public class TypeDoorEntryDevice : TypeDeviceInput
    {
        public TypeDoorEntryDevice()
        {
            TypeDevice = AppConsts.DoorEntryDevice;
        }
    }

    [AutoMap(typeof(TypeDeviceInput))]
    public class TypeConditionerDevice : TypeDeviceInput
    {
        public TypeConditionerDevice()
        {
            TypeDevice = AppConsts.ConditionerDevice;
        }
    }

    [AutoMap(typeof(TypeDeviceInput))]
    public class TypeConnectionDevice : TypeDeviceInput
    {
        public TypeConnectionDevice()
        {
            TypeDevice = AppConsts.ConnectionDevice;
        }
    }

    [AutoMap(typeof(TypeDeviceInput))]
    public class TypeFireAlarmDevice : TypeDeviceInput
    {
        public TypeFireAlarmDevice()
        {
            TypeDevice = AppConsts.FireAlarmDevice;
        }
    }

    [AutoMap(typeof(TypeDeviceInput))]
    public class TypeSecurityDevice : TypeDeviceInput
    {
        public TypeSecurityDevice()
        {
            TypeDevice = AppConsts.SecurityDevice;
        }
    }

    [AutoMap(typeof(TypeDeviceInput))]
    public class TypeWaterDevice : TypeDeviceInput
    {
        public TypeWaterDevice()
        {
            TypeDevice = AppConsts.WaterDevice;
        }
    }

    public class SmartHomeUpdate
    {
        public SmartHomeDto Smarthome { get; set; }
        public List<RoomUpdate> Rooms { get; set; }
    }

    public class FloorUpdate : FloorDto
    {
        public List<RoomUpdate> Rooms { get; set; }
    }

    public class RoomUpdate : RoomDto
    {
        public List<TypeLightingDevice> LightingIds { get; set; }
        public List<TypeCurtainDevice> CurtainIds { get; set; }
        public List<TypeAirDevice> AirIds { get; set; }
        public List<TypeWaterDevice> WatterIds { get; set; }
        public List<TypeDoorEntryDevice> DoorEntryIds { get; set; }
        public List<TypeConnectionDevice> ConnectionIds { get; set; }
        public List<TypeConditionerDevice> ConditionerIds { get; set; }
        public List<TypeFireAlarmDevice> FireAlarmIds { get; set; }
        public List<TypeSecurityDevice> SecurityIds { get; set; }
        public List<HomeDeviceDto> Devices { get; set; }
      

    }

    [AutoMap(typeof(FloorSmartHome))]
    public class FloorDto : FloorSmartHome
    {

    }

    [AutoMap(typeof(RoomSmartHome))]
    public class RoomDto : RoomSmartHome
    {

    }

    public class GetRoomInput
    {
        public int? FormCase { get; set; }
        public long? SmartHomeId { get; set; }
        public long? FloorSmartHomeId { get; set; }

    }

    public class DataSearch {
        public string data { get; set; }
    
    }

    [AutoMap(typeof(HomeDevice))]
    public class HomeDeviceDto : HomeDevice
    {
        public List<SmarthomeApi> ListApis { get; set; }
    }
   


}
