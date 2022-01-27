using Abp;
using Abp.RealTime;
using MHPQ.EntityDb;
using MHPQ.Friendships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Chat
{
    public interface IChatCommunicator
    {
      

        #region Chatp2p
        void SendMessageToClient(IReadOnlyList<IOnlineClient> clients, ChatMessage message);

        void SendFriendshipRequestToClient(IReadOnlyList<IOnlineClient> clients, Friendship friend, bool isOwnRequest, bool isFriendOnline);

        void SendUserConnectionChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, bool isConnected);

        void SendUserStateChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, FriendshipState newState);

        void SendAllUnreadMessagesOfUserReadToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user);
        #endregion

        #region ChatRoom
        void AddUserToGroupChat(IReadOnlyList<IOnlineClient> clients, string roomChatCode, UserIdentifier user);

        void RemoveUserFromGroupChat(IReadOnlyList<IOnlineClient> clients, string roomChatCode, UserIdentifier user);

        void SendMessageToGroupChatClient(string roomChatCode, RoomMessage message);

        void SendNotificationAddGroupToUserClient(string roomChatCode, UserIdentifier user);

        void SendNotificationCreateGroupToUserClient(string roomChatCode, UserIdentifier user);

        void SendAllUnreadRoomMessagesOfUserReadToClients(string roomChatCode, UserIdentifier user);
        #endregion
    }
}
