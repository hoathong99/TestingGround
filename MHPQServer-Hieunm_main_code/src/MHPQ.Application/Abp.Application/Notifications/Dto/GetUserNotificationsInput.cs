using Abp.Notifications;


namespace MHPQ.Notifications.Dto
{
    public class GetUserNotificationsInput 
    {
        public UserNotificationState? State { get; set; }
    }
}