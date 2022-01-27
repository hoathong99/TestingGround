using Abp.RealTime;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Notifications
{
    public interface IBusinessCommunicator
    {
        void SendOrderToPartnerClient(IReadOnlyList<IOnlineClient> clients, Order order);
        void SendMessageToUserClient(IReadOnlyList<IOnlineClient> clients, string messager);
    }
}
