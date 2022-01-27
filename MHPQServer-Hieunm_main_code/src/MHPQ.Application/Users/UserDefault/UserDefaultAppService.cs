using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.IO;
using Abp.Runtime.Session;
using Abp.UI;
using MHPQ.Authorization;
using MHPQ.Authorization.Roles;
using MHPQ.Authorization.Users;
using MHPQ.Roles.Dto;
using MHPQ.Storage;
using MHPQ.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Users
{
    public interface IUserDefaultAppService : IApplicationService
    {

        //  Task<ListResultDto<RoleDto>> GetRoles();
        // Task ChangeLanguage(ChangeUserLanguageDto input);
        Task<UserDto> GetDetail();
        Task UpdateProfilePicture(UpdateProfilePictureInput input);
        Task<bool> ChangePassword(ChangePasswordDto input);
    }
    [AbpAuthorize(PermissionNames.Pages_User_Detail)]
    public class UserDefaultAppService : ApplicationService, IUserDefaultAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IAppFolders _appFolders;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public UserDefaultAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IAppFolders appFolders,
            IBinaryObjectManager binaryObjectManager)
           
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _appFolders = appFolders;
            _binaryObjectManager = binaryObjectManager;
        }

       

        public async Task<UserDto> GetDetail()
        {
            // CheckGetPermission();
            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());

            //chuyển về dạng Dto để chuyển dữ liệu sang json và lưu vào qrcode
            var userDto = MapToEntityDto(user);
            //var data = JsonConvert.SerializeObject(userDto, Formatting.None,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });
            //if (userDto.QRCodeBase64 == null)
            //{
            //    //Tạo QR code
            //    QRCodeGenerator QrGenerator = new QRCodeGenerator();
            //    QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(userDto.Id.ToString(), QRCodeGenerator.ECCLevel.Q);
            //    QRCode QrCode = new QRCode(QrCodeInfo);
            //    Bitmap QrBitmap = QrCode.GetGraphic(60);
            //    byte[] BitmapArray = QrBitmap.BitmapToByteArray();
            //    user.QRCodeBase64 = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
            //}

            return MapToEntityDto(user);
        }

        public async Task UpdateProfilePicture(UpdateProfilePictureInput input)
        {
            var tempProfilePicturePath = Path.Combine(_appFolders.TempFileDownloadFolder, input.FileName);

            byte[] byteArray;

            using (var fsTempProfilePicture = new FileStream(tempProfilePicturePath, FileMode.Open))
            {
                using (var bmpImage = new Bitmap(fsTempProfilePicture))
                {
                    var width = input.Width == 0 ? bmpImage.Width : input.Width;
                    var height = input.Height == 0 ? bmpImage.Height : input.Height;
                    var bmCrop = bmpImage.Clone(new Rectangle(input.X, input.Y, width, height), bmpImage.PixelFormat);

                    using (var stream = new MemoryStream())
                    {
                        bmCrop.Save(stream, bmpImage.RawFormat);
                        stream.Close();
                        byteArray = stream.ToArray();
                    }
                }
            }

            if (byteArray.LongLength > 102400) //100 KB
            {
                throw new UserFriendlyException(L("ResizedProfilePicture_Warn_SizeLimit"));
            }

            var user = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());

            if (user.ProfilePictureId.HasValue)
            {
                await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
            await _binaryObjectManager.SaveAsync(storedFile);

            user.ProfilePictureId = storedFile.Id;

            FileHelper.DeleteIfExists(tempProfilePicturePath);
        }




       

        protected  User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected  void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected  UserDto MapToEntityDto(User user)
        {
            if (user.Roles != null)
            {
                var roleIds = user.Roles.Select(x => x.RoleId).ToArray();
                var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);
                var userDto = ObjectMapper.Map<UserDto>(user);
                userDto.RoleNames = roles.ToArray();
                return userDto;
            }
            else
            {
                var userDto = ObjectMapper.Map<UserDto>(user);
                return userDto;
            }
        }

    

     

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Incorrect password."
                }));
            }

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attempting to reset password.");
            }

            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }

            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return true;
        }
    }


}
