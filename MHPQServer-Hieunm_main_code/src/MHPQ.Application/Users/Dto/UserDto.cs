using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using MHPQ.Authorization.Users;
using Newtonsoft.Json;

namespace MHPQ.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
       
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }
    
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }
        public string FullName { get; set; }
        //[JsonProperty]
        public string PhoneNumber { get; set; }

        public DateTime? LastLoginTime { get; set; }
        public DateTime CreationTime { get; set; }
        public string[] RoleNames { get; set; }

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
        
        public string IdentityNumber { get; set; }
       
        public string QRCodeBase64 { get; set; }
    }
}
