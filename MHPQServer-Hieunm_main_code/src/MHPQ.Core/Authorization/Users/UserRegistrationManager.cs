using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization.Users;
using Abp.Domain.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Abp.UI;
using MHPQ.Authorization.Roles;
using MHPQ.MultiTenancy;
using MHPQ.EntityDb;
using Abp.Domain.Repositories;

namespace MHPQ.Authorization.Users
{
    public class UserRegistrationManager : DomainService
    {
        public IAbpSession AbpSession { get; set; }

        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<Citizen, long> _citizenRepos;

        public UserRegistrationManager(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IPasswordHasher<User> passwordHasher,
            IRepository<Citizen, long> citizenRepos
            )
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _citizenRepos = citizenRepos;
            AbpSession = NullAbpSession.Instance;
        }

        public async Task<User> RegisterAsync(string name, string surname, string emailAddress, string userName, string plainPassword, bool isEmailConfirmed, bool isCitizen)
        {
            //CheckForTenant();

            //var tenant = await GetActiveTenantAsync();

            var user = new User
            {
                Name = name,
                Surname = surname,
                EmailAddress = emailAddress,
                IsActive = true,
                UserName = userName,
                IsEmailConfirmed = isEmailConfirmed,
                Roles = new List<UserRole>()
            };

           

            user.SetNormalizedNames();
            var defaultRole = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == StaticRoleNames.Tenants.DefaultUser && r.TenantId == AbpSession.TenantId);
            if(defaultRole != null)
            {
                user.Roles.Add(new UserRole(null, user.Id, defaultRole.Id));
            }

            await _userManager.InitializeOptionsAsync(null);

            CheckErrors(await _userManager.CreateAsync(user, plainPassword));
            if (AbpSession.TenantId.HasValue)
            {
                var citizen = new Citizen()
                {
                    FullName =  name + " " + surname,
                    AccountId = user.Id,
                    Email = emailAddress,
                    TenantId = AbpSession.TenantId

                };
               await _citizenRepos.InsertAsync(citizen);
            }
           
            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        private void CheckForTenant()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new InvalidOperationException("Can not register host users!");
            }
        }

        private async Task<Tenant> GetActiveTenantAsync()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return await GetActiveTenantAsync(AbpSession.TenantId.Value);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
