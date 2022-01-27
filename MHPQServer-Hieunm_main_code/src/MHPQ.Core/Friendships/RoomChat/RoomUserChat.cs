using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using MHPQ.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.RoomChats
{
    [Table("AppRoomUserChats")]
    public class RoomUserChat : Entity<long>, IHasCreationTime, IMayHaveTenant
    {
        [Key, Column(Order = 1)]
        public long UserId { get; set; }
        [Key, Column(Order = 2)]
        public long RoomChatId { get; set; }

        public DateTime CreationTime { get; set; }

        public int? TenantId { get; set; }

        public User User { get; set; }

        public RoomChat RoomChat { get; set; }
    }
}
