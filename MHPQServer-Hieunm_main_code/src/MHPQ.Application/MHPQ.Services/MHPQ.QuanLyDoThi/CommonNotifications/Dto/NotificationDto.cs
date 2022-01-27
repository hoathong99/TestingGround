using Abp.AutoMapper;
using MHPQ.EntityDb;
using System;

namespace MHPQ.Services
{
    [AutoMap(typeof(CityNotification))]
    public class CityNotificationDto : CityNotification
    {
    }

   

    [AutoMap(typeof(CityNotificationComment))]
    public class CommentDto : CityNotificationComment
    {

    }

    public class UserOffline
    {
        public long Id { get; set; }
        public bool IsOnline { get; set; }
    }

    public class NotificationInput
    {
        public int Type { get; set; }
    }

  
}
