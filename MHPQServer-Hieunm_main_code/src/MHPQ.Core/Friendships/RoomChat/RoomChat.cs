using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using MHPQ.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MHPQ.RoomChats
{

    [Table("AppRoomChats")]
    public class RoomChat : Entity<long>, IHasCreationTime, IMayHaveTenant
    {
        public long AdminId { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string RoomChatCode { get; set; }

        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string AdminName { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public DateTime CreationTime { get; set; }

       // public ICollection<User> Users { get; set; }
        public List<RoomUserChat> RoomUserChats { get; set; }

        public RoomChat(UserIdentifier user, string GroupName, Guid? probableFriendProfilePictureId)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }


            AdminId = user.UserId;
            TenantId = user.TenantId;
            ProfilePictureId = probableFriendProfilePictureId;
            Name = GroupName;
            CreationTime = Clock.Now;
        }

        protected RoomChat()
        {

        }
    }
}
