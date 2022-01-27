using Abp.RealTime;
using MHPQ.Authorization.Users;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Notifications
{
    public interface INotificationCommunicator
    {
        void SendNotificaionToUserTenant(IReadOnlyList<IOnlineClient> clients, CityNotification noti);
        void SendCommentFeedbackToUserTenant(IReadOnlyList<IOnlineClient> clients, UserFeedbackComment noti);
        void SendCommentFeedbackToAdminTenant(IReadOnlyList<User> clients, UserFeedbackComment noti);
        void SendNotificationToAdminTenant(IReadOnlyList<User> clients, UserFeedback noti);
    }
}
