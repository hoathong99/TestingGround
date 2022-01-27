using Abp.AutoMapper;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;

namespace MHPQ.Services.Dto
{

    public class RoomSmartHomeInput
    {
    }

    public class GetAllRoomInput
    {
        public long? SmartHomeId { get; set; }
    }

    public class CreateRoomInput
    {
        public RoomDto Room { get; set; }
        public List<DeviceDto> Devices { get; set; }
    }

    public class UpdateRoomInput
    {
        public  RoomDto Room { get; set; }
        public  List<HomeDevice> Devices { get; set; }
    }

}