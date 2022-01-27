using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using MHPQ.RoomChats;
using System;
using System.ComponentModel.DataAnnotations;

namespace MHPQ.Friendships.Dto
{
    [AutoMapFrom(typeof(RoomChat))]
    public class RoomChatDto: EntityDto<long>
    {
        public long AdminId { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string AdminName { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public DateTime CreationTime { get; set; }

        public int? UnreadMessageCount { get; set; }

        public bool? IsOnline { get; set; }
    }
}
