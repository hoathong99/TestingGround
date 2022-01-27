using Abp.Dependency;
using Abp.RealTime;
using Castle.Core.Logging;
using MHPQ.EntityDb;
using MHPQ.Notifications;
using MHPQ.Services.Dto;
using MHPQ.Web.Host.Chat;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace MHPQ.Web.Host.SignalR
{
    public class BusinessCommunicator : IBusinessCommunicator, ITransientDependency
    {
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        private static IHubContext<ChatHub> _businessHub;
        private readonly IOnlineClientManager _onlineClientManager;

        public BusinessCommunicator(
            IOnlineClientManager onlineClientManager,
            IHubContext<ChatHub> businessHub)
        {
            Logger = NullLogger.Instance;
            _onlineClientManager = onlineClientManager;
            _businessHub = businessHub;
        }

        [System.Obsolete]
        public void SendOrderToPartnerClient(IReadOnlyList<IOnlineClient> clients, Order order)
        {

            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }

                // signalRClient.getUserConnectNotification(user, isConnected);
                _businessHub.Clients.Client(client.ConnectionId).SendAsync("SendNotifierPartner", order);
            }


        }

        [System.Obsolete]
        public void SendMessageToUserClient(IReadOnlyList<IOnlineClient> clients, string messager)
        {

            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }

                // signalRClient.getUserConnectNotification(user, isConnected);
                _businessHub.Clients.Client(client.ConnectionId).SendAsync("SendMessageToUserClient", messager);
            }


        }



        private dynamic GetSignalRClientOrNull(IOnlineClient client)
        {
            var signalRClient = _businessHub.Clients.Client(client.ConnectionId);
            if (signalRClient == null)
            {
                Logger.Debug("Can not get chat user " + client.UserId + " from SignalR hub!");
                return null;
            }

            return signalRClient;
        }
    }
}