using Abp;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.RealTime;
using Abp.Runtime.Session;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPush;

namespace MHPQ.Services
{
    public interface IPushNotificationAppService : IApplicationService
    {
        Task<object> CreateSubscriptionClient(PushSubscription sub);
        Task<object> Broadcast(string message);
    }

    public class PushNotificationAppService : MHPQAppServiceBase, IPushNotificationAppService
    {

        private static List<PushSubscription> Subscriptions = new List<PushSubscription>();

        private readonly IOnlineClientManager _onlineClientManager;
        private readonly INotificationCommunicator _notificationCommunicator;
        private readonly IRepository<WebPushEndPoint, long> _endpointRepos;
        public PushNotificationAppService(
            IOnlineClientManager onlineClientManager,
            INotificationCommunicator notificationCommunicator,
            IRepository<WebPushEndPoint, long> endpointRepos
            )
        {
            _onlineClientManager = onlineClientManager;
            _notificationCommunicator = notificationCommunicator;
            _endpointRepos = endpointRepos;
        }

        [Obsolete]
        public async Task<object> CreateSubscriptionClient(PushSubscription sub)
        {
            try
            {
                var insertInput = sub.MapTo<WebPushEndPoint>();
                long id = await _endpointRepos.InsertAndGetIdAsync(insertInput);

              
                var data = DataResult.ResultSucces(insertInput, "Insert success !");
                return data;

            }
            catch(Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "exception !");
                Logger.Fatal(e.Message, e);
                return data;
            }
        }


        public async Task<object> Broadcast(string message)
        {
           try
            {
                var subject = "mailto:mail@example.com";
                var publicKey = "BNOJyTgwrEwK9lbetRcougxkRgLpPs1DX0YCfA5ZzXu4z9p_Et5EnvMja7MGfCqyFCY4FnFnJVICM4bMUcnrxWg";
                var privateKey = "_kRzHiscHBIGftfA7IehH9EA3RvBl8SBYhXBAMz6GrI";

                var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
                var client = new WebPushClient();
                message = "Hello world !" + new Random().ToString();
                var mes = new NotificationModel()
                {
                    body = message,
                    title = "Notification",
                    icon = "assets/img/user.png"
                };

                var a = new Noti
                {
                    notification = mes
                };

                var serializedMessage = JsonConvert.SerializeObject(a);
                foreach(var pushSubscription in Subscriptions)
                {
                    if (pushSubscription.Auth != null && pushSubscription.Endpoint != null && pushSubscription.P256DH != null)
                    {
                        try
                        {
                            client.SendNotification(pushSubscription, serializedMessage, vapidDetails);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
               
                var data = DataResult.ResultSucces(serializedMessage, "Subscriptions success !");
                return data;
            }
            catch(Exception ex)
            {
                var data = DataResult.ResultError(null, "Want to login!");
                return data;
            }
        }
    }
}
