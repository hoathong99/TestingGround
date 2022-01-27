using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MHPQ.RoomChats;
using System;

namespace MHPQ.Friendships.Dto
{

    [AutoMapFrom(typeof(RoomUserChat))]
    public class RoomChatUserDto : EntityDto
    {
        public long UserId { get; set; }

        public long RoomChatId { get; set; }

        public DateTime CreationTime { get; set; }

        public int? TenantId { get; set; }
    }
}
