using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Extensions;
using MHPQ.RoomChats;
using Newtonsoft.Json;

namespace MHPQ.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        //public ICollection<RoomChat> RoomChats { get; set; }

        public List<RoomUserChat> RoomUserChats { get; set; }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
        //Thong tin dan cu
        [StringLength(1000)]
        public string HomeAddress { get; set; }
        [StringLength(1000)]
        public string AddressOfBirth { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(128)]
        public string Gender { get; set; }
        [StringLength(256)]
        public string Nationality { get; set; }
        public virtual Guid? ProfilePictureId { get; set; }
        [StringLength(256)]
        public string ImageUrl { get; set; }
        public long? PhanKhuId { get; set; }
        public long? HouseId { get; set; }

        // public long? GroupUserId { get; set; }

        public string IdentityNumber { get; set; }
        /// <summary>
        /// QR code
        /// </summary>'
        //public string QRCodeBase64 { get; set; }

    }
}
