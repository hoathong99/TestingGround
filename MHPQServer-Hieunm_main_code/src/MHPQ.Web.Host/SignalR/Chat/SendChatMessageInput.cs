using System;

namespace MHPQ.Web.Host.Chat
{
    public class SendChatMessageInput
    {
        public int? TenantId { get; set; }

        public long UserId { get; set; }

        public long SenderId { get; set; }

        public string UserName { get; set; }

        public string TenancyName { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public string Message { get; set; }
    }


    public class SendGroupChatMessageInput
    {
        public int? TenantId { get; set; }

        public long UserId { get; set; }

        public long SenderId { get; set; }

        public string UserName { get; set; }

        public string TenancyName { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public string Message { get; set; }
    }
}