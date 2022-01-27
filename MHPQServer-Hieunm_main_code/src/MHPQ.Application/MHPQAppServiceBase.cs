using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using MHPQ.Authorization.Users;
using MHPQ.MultiTenancy;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Concurrent;
using MHPQ.EntityDb;
using System.Collections.Generic;
using MHPQ.Services;

namespace MHPQ
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class MHPQAppServiceBase : ApplicationService
    {
        public static Benchmark mb = new Benchmark();
        private static ConcurrentDictionary<long, List<CityNotificationDto>> UserCityNotifications = new ConcurrentDictionary<long, List<CityNotificationDto>>();
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected MHPQAppServiceBase()
        {
            LocalizationSourceName = MHPQConsts.LocalizationSourceName;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }


        protected virtual string GetUniqueKey()
        {
            int maxSize = 10;
            char[] chars = new char[36];
            string a;
            a = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            return result.ToString();
        }
    }
}
