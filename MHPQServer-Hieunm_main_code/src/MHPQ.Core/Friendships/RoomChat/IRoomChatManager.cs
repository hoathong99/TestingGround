using Abp;
using Abp.Domain.Services;
using MHPQ.Chat;
using MHPQ.Friendships;
using System;
using System.Collections.Generic;

namespace MHPQ.RoomChats
{
    public interface IRoomChatManager : IDomainService
    {

        void SendGroupChatMessage(UserIdentifier sender, string roomChatId, string message, string senderTenancyName, string senderUserName, Guid? senderProfilePictureId);

        void  AddMembershipGroupChat(UserIdentifier member, long groupId);
        RoomUserChat GetMemberGroupChatOrNull(UserIdentifier member, long groupId);
        long CreatGroupChat(RoomChat room);
        RoomChat GetRoomChat(long roomId);     
    }
}
