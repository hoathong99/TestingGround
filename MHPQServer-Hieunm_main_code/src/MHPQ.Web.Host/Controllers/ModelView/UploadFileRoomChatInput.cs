﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Web.Host.ModelView
{

    public class UploadFileRoomChatInput
    {
        public int RoomId { get; set; }
        public IFormFile File { get; set; }
    }


    public class UploadImageUserChatInput
    {
        public int? TenantId { get; set; }

        public string UserId { get; set; }

        public long SenderId { get; set; }

        public string UserName { get; set; }

        public string TenancyName { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public IFormFile File { get; set; }
    }
}
