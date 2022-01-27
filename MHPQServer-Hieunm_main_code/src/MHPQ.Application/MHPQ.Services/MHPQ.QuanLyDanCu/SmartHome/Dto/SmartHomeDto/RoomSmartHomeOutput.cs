using Abp.AutoMapper;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;

namespace MHPQ.Services.Dto
{

    public class RoomSmartHomeOutput
    {
    }

    public class GetAllRoomOutput
    {
        public List<RoomSmartHome> Rooms { get; set; }
        public string Message { get; set; }

    }

    public class GetRoomByIdOutput {
        public RoomSmartHome Room { get; set; }
        public string Message { get; set; }
    }

    public class CreateRoomOutput {
        public RoomSmartHome Room { get; set; }
        public int Created { get; set; }
        public string Message { get; set; }
    }

    public class UpdateRoomOutput {
        public RoomSmartHome Room { get; set; }
        public int Updated { get; set; }
        public string Message { get; set; }
    }

    public class DeleteRoomOutput {
        public int Deleted { get; set; }
        public string Message { get; set; }
    }
}