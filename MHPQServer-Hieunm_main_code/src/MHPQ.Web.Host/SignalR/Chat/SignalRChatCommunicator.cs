using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.RealTime;
using Castle.Core.Logging;
using MHPQ.Chat;
using MHPQ.Chat.Dto;
using MHPQ.EntityDb;
using MHPQ.Friendships;
using MHPQ.Friendships.Dto;
using MHPQ.Services;
using Microsoft.AspNetCore.SignalR;

namespace MHPQ.Web.Host.Chat
{
    public class SignalRChatCommunicator : IChatCommunicator, ITransientDependency
    {
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        private static IHubContext<ChatHub> ChatHub;
        private readonly IOnlineClientManager _onlineClientManager;

        public SignalRChatCommunicator(
            IOnlineClientManager onlineClientManager,
            IHubContext<ChatHub> chatHub)
        {
            Logger = NullLogger.Instance;
            _onlineClientManager = onlineClientManager;
            ChatHub = chatHub;
        }

      


        [System.Obsolete]
        public void SendMessageToClient(IReadOnlyList<IOnlineClient> clients, ChatMessage message)
        {

            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    return;
                }
                ChatHub.Clients.Client(client.ConnectionId).SendAsync("SendMessageToClient", message.MapTo<ChatMessageDto>());
            }

        }

        [System.Obsolete]
        public void SendFriendshipRequestToClient(IReadOnlyList<IOnlineClient> clients, Friendship friendship, bool isOwnRequest, bool isFriendOnline)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    return;
                }

                var friendshipRequest = friendship.MapTo<FriendDto>();
                friendshipRequest.IsOnline = isFriendOnline;

                //signalRClient.getFriendshipRequest(friendshipRequest, isOwnRequest);
                ChatHub.Clients.Client(client.ConnectionId).SendAsync("SendFriendshipRequestToClient", friendshipRequest, isOwnRequest);
            }
        }

        public void SendUserConnectionChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, bool isConnected)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }

               // signalRClient.getUserConnectNotification(user, isConnected);
                ChatHub.Clients.Client(client.ConnectionId).SendAsync("GetUserConnectNotification", user, isConnected);
            }
        }

        public void SendUserStateChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, FriendshipState newState)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }

                //signalRClient.getUserStateChange(user, newState);
                ChatHub.Clients.Client(client.ConnectionId).SendAsync("GetUserStateChange", user, newState);

            }
        }

        public void SendAllUnreadMessagesOfUserReadToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }
                ChatHub.Clients.Client(client.ConnectionId).SendAsync("SendAllUnreadMessagesOfUserReadToClients", user);
               // signalRClient.getallUnreadMessagesOfUserRead(user);
            }
        }


        private dynamic GetSignalRClientOrNull(IOnlineClient client)
        {
            var signalRClient = ChatHub.Clients.Client(client.ConnectionId);
            if (signalRClient == null)
            {
                Logger.Debug("Can not get chat user " + client.UserId + " from SignalR hub!");
                return null;
            }

            return signalRClient;
        }


        #region ChatRoom

        [System.Obsolete]
        public void SendMessageToGroupChatClient(string roomChatCode, RoomMessage message)
        {
            ChatHub.Clients.Group(roomChatCode).SendAsync("SendMessageToGroupChatClient", message.MapTo<RoomMessageDto>());
        }

        public void SendNotificationAddGroupToUserClient(string roomChatCode, UserIdentifier user)
        {
            ChatHub.Clients.Group(roomChatCode).SendAsync("SendFriendshipRequestToClient", user);
        }

        public void SendAllUnreadRoomMessagesOfUserReadToClients(string roomChatCode, UserIdentifier user)
        {
            ChatHub.Clients.Group(roomChatCode).SendAsync("SendAllUnreadRoomMessagesOfUserReadToClients", user);
        }

        public void AddUserToGroupChat(IReadOnlyList<IOnlineClient> clients, string roomChatCode, UserIdentifier user)
        {
            foreach(var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    return;
                }
                ChatHub.Groups.AddToGroupAsync(client.ConnectionId, roomChatCode);
                
            }
        }


        public void SendNotificationCreateGroupToUserClient(string roomChatCode, UserIdentifier user)
        {
            ChatHub.Clients.Group(roomChatCode).SendAsync("SendNotificationCreateGroupToUserClient", user);
        }

        public void RemoveUserFromGroupChat(IReadOnlyList<IOnlineClient> clients, string roomChatCode, UserIdentifier user)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    return;
                }
                ChatHub.Groups.RemoveFromGroupAsync(client.ConnectionId, roomChatCode);

            }
        }

        #endregion

    }
}