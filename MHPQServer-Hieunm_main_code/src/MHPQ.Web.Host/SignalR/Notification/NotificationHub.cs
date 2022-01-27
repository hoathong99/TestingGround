using Abp.AspNetCore.SignalR.Hubs;
using Abp.Dependency;
using Abp.Localization;
using Abp.RealTime;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHPQ.Web.Host.SignalR
{
    public class NotificationHub : OnlineClientHubBase, ITransientDependency
    {


        private readonly ILocalizationManager _localizationManager;


        [Obsolete]
        public NotificationHub(
            ILocalizationManager localizationManager,
            IOnlineClientManager onlineClientManager,
            IOnlineClientInfoProvider clientInfoProvider) : base(onlineClientManager, clientInfoProvider)
        {
            _localizationManager = localizationManager;


            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        [Obsolete]
        public string NotificationCity(CommonNotificationInput input)
        {
            try
            {


                return string.Empty;
            }
            catch (Exception ex)
            {

                Logger.Warn(ex.ToString(), ex);
                return _localizationManager.GetSource("AbpWeb").GetString("InternalServerError");
            }
        }
    }

}
