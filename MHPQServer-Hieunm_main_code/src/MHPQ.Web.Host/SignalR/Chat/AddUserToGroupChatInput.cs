using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHPQ.Web.Host.Chat
{
    public class AddUserToGroupChatInput
    {
        public long GroupChatId { get; set; }
        public string GroupName { get; set; }
        public string RoomChatCode { get; set; }
        public List<long> ListFriendIds { get; set; }

    }
}
