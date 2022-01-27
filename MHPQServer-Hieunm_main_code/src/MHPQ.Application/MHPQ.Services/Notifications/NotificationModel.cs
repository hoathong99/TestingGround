using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public class Noti
    {
        public NotificationModel notification { get; set; }
    }
    public class NotificationModel
    {
        public string title { get; set; }
        public string body { get; set; }
        public string icon { get; set; }
    }
}
