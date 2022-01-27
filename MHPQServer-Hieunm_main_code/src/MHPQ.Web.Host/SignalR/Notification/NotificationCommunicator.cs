using Abp.Dependency;
using Abp.RealTime;
using Castle.Core.Logging;
using MHPQ.EntityDb;
using MHPQ.Notifications;
using MHPQ.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Abp.AutoMapper;
using MHPQ.Web.Host.Chat;
using MHPQ.Authorization.Users;

namespace MHPQ.Web.Host.SignalR
{
    public class NotificationCommunicator : INotificationCommunicator, ITransientDependency
    {
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        private static IHubContext<ChatHub> _notificationHub;
        private readonly IOnlineClientManager _onlineClientManager;

        public NotificationCommunicator(
            IOnlineClientManager onlineClientManager,
            IHubContext<ChatHub> notificationHub)
        {
            Logger = NullLogger.Instance;
            _onlineClientManager = onlineClientManager;
            _notificationHub = notificationHub;
        }

        [System.Obsolete]
        public void SendNotificaionToUserTenant(IReadOnlyList<IOnlineClient> clients, CityNotification noti)
        {

            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }

                // signalRClient.getUserConnectNotification(user, isConnected);
                _notificationHub.Clients.Client(client.ConnectionId).SendAsync("SendNotificaionToUserTenant", noti.MapTo<CityNotificationDto>());
            }

        }

        [System.Obsolete]
        public void SendNotificationToAdminTenant(IReadOnlyList<User> clients, UserFeedback noti)
        {
            
            foreach (var client in clients)
            {
                //var signalRClient = GetSignalRClientOrNull(client);
                //if (signalRClient == null)
                //{
                //    continue;
                //}

                // signalRClient.getUserConnectNotification(user, isConnected);
                //_notificationHub.Clients.Client(client.ConnectionId).SendAsync("SendNotificationToAdminTenant", noti.MapTo<UserFeedbackDto>());

                _notificationHub.Clients.User(client.Id.ToString()).SendAsync("SendNotificationToAdminTenant", noti.MapTo<UserFeedbackDto>());

            }
        }

        private dynamic GetSignalRClientOrNull(IOnlineClient client)
        {
            var signalRClient = _notificationHub.Clients.Client(client.ConnectionId);
            if (signalRClient == null)
            {
                Logger.Debug("Can not get chat user " + client.UserId + " from SignalR hub!");
                return null;
            }

            return signalRClient;
        }

        [System.Obsolete]
        public void SendCommentFeedbackToUserTenant(IReadOnlyList<IOnlineClient> clients, UserFeedbackComment noti)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }

                // signalRClient.getUserConnectNotification(user, isConnected);
                _notificationHub.Clients.Client(client.ConnectionId).SendAsync("SendCommentFeedbackToUserTenant", noti.MapTo<UserFeedbackCommentDto>());
            }
        }

        [System.Obsolete]
        public void SendCommentFeedbackToAdminTenant(IReadOnlyList<User> clients, UserFeedbackComment noti)
        {
            foreach (var client in clients)
            {
                _notificationHub.Clients.User(client.Id.ToString()).SendAsync("sendcmfbtoadtenant", noti.MapTo<UserFeedbackCommentDto>());

            }
        }
    }
}