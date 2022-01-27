using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Chat
{

    [Table("AppRoomMessages")]
    public class RoomMessage : Entity<long>, IHasCreationTime, IMayHaveTenant
    {
        public const int MaxMessageLength = 4 * 1024; //4KB

        public long UserId { get; set; }

        public int? TenantId { get; set; }

        public long GroupId { get; set; }

        [Required]
        [StringLength(MaxMessageLength)]
        public string Message { get; set; }

        public DateTime CreationTime { get; set; }

        public ChatSide Side { get; set; }

        public ChatMessageReadState ReadState { get; private set; }

        public RoomMessage(
            UserIdentifier user,
            long groupid,
            ChatSide side,
            string message,
            ChatMessageReadState readState)
        {
            UserId = user.UserId;
            TenantId = user.TenantId;
            GroupId = groupid;
            Message = message;
            Side = side;
            ReadState = readState;

            CreationTime = Clock.Now;
        }

        public void ChangeReadState(ChatMessageReadState newState)
        {
            ReadState = newState;
        }

        protected RoomMessage()
        {

        }
    }
}
